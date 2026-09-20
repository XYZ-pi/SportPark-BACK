using Microsoft.EntityFrameworkCore;
using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;

namespace SportPark.BusinessLogic
{
    public class BookingManagementService
    {
        private readonly AppDbContext _context;
        private readonly SubscriptionManagementService _subscriptionService;

        public BookingManagementService(AppDbContext context, SubscriptionManagementService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public async Task<BookingResponse> Create(int userId, BookingCreateRequest request)
        {
            var session = await _context.ClassSessions
                .Include(cs => cs.Service)
                .Include(cs => cs.Trainer)
                    .ThenInclude(t => t!.User)
                .FirstOrDefaultAsync(cs => cs.Id == request.ClassSessionId);

            if (session == null)
                throw new InvalidOperationException("Занятие не найдено");

            var alreadyBooked = await _context.Bookings.AnyAsync(b =>
                b.UserId == userId &&
                b.ClassSessionId == request.ClassSessionId &&
                b.Status != BookingStatus.Cancelled);

            if (alreadyBooked)
                throw new InvalidOperationException("Вы уже записаны на это занятие");

            var subscription = await _subscriptionService.GetActiveSubscription(userId, SubscriptionType.Group);
            if (subscription == null)
                throw new InvalidOperationException("Нет активного группового абонемента с оставшимися посещениями");

            var booking = new Booking
            {
                UserId = userId,
                ClassSessionId = request.ClassSessionId,
                BookedAt = DateTime.UtcNow,
                Status = BookingStatus.Confirmed
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return MapToResponse(booking, session);
        }

        public async Task<List<BookingResponse>> GetMyBookings(int userId)
        {
            return await _context.Bookings
                .Include(b => b.ClassSession)
                    .ThenInclude(cs => cs!.Service)
                .Include(b => b.ClassSession)
                    .ThenInclude(cs => cs!.Trainer)
                        .ThenInclude(t => t!.User)
                .Where(b => b.UserId == userId)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    ServiceName = b.ClassSession!.Service!.Name,
                    TrainerName = b.ClassSession.Trainer!.User!.Name,
                    DayOfWeek = b.ClassSession.DayOfWeek,
                    StartTime = b.ClassSession.StartTime.ToString(@"hh\:mm"),
                    Hall = b.ClassSession.Hall,
                    BookedAt = b.BookedAt,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<bool> Cancel(int userId, int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.ClassSession)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null) return false;
            if (booking.Status != BookingStatus.Confirmed) return false;

            var sessionDateTime = GetNextOccurrenceUtc(booking.ClassSession!.DayOfWeek, booking.ClassSession.StartTime);
            var isLateCancel = (sessionDateTime - DateTime.Now).TotalHours < 2;

            booking.Status = BookingStatus.Cancelled;

            if (isLateCancel)
            {
                await DeductSession(userId, SubscriptionType.Group);
                booking.SessionDeducted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // requestingUserId — тот, кто выполняет действие (тренер или админ); isAdmin — обходит проверку "сегодня"
        public async Task<bool> MarkCompleted(int requestingUserId, bool isAdmin, int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.ClassSession)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return false;
            if (booking.Status != BookingStatus.Confirmed) return false;

            if (!isAdmin)
            {
                var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == requestingUserId);
                if (trainer == null || trainer.Id != booking.ClassSession!.TrainerId)
                    return false; // не тот тренер

                if (booking.ClassSession!.DayOfWeek != DateTime.Now.DayOfWeek)
                    return false; // не сегодняшнее занятие
            }

            booking.Status = BookingStatus.Completed;
            booking.SessionDeducted = true;
            await DeductSession(booking.UserId, SubscriptionType.Group);

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task DeductSession(int userId, SubscriptionType type)
        {
            var subscription = await _subscriptionService.GetActiveSubscription(userId, type);
            if (subscription != null)
            {
                subscription.RemainingSessions -= 1;
                if (subscription.RemainingSessions <= 0)
                    subscription.IsActive = false;
            }
        }

        private static DateTime GetNextOccurrenceUtc(DayOfWeek day, TimeSpan time)
        {
            var now = DateTime.Now;
            int daysUntil = ((int)day - (int)now.DayOfWeek + 7) % 7;
            var candidate = now.Date.AddDays(daysUntil).Add(time);
            if (daysUntil == 0 && candidate < now)
                candidate = candidate.AddDays(7);
            return candidate;
        }

        private static BookingResponse MapToResponse(Booking booking, ClassSession session)
        {
            return new BookingResponse
            {
                Id = booking.Id,
                ServiceName = session.Service!.Name,
                TrainerName = session.Trainer!.User!.Name,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime.ToString(@"hh\:mm"),
                Hall = session.Hall,
                BookedAt = booking.BookedAt,
                Status = booking.Status.ToString()
            };
        }

        public async Task<List<BookingResponse>> GetAll()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.ClassSession)
                    .ThenInclude(cs => cs!.Service)
                .Include(b => b.ClassSession)
                    .ThenInclude(cs => cs!.Trainer)
                        .ThenInclude(t => t!.User)
                .OrderByDescending(b => b.BookedAt)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    ServiceName = b.ClassSession!.Service!.Name,
                    TrainerName = b.ClassSession.Trainer!.User!.Name,
                    DayOfWeek = b.ClassSession.DayOfWeek,
                    StartTime = b.ClassSession.StartTime.ToString(@"hh\:mm"),
                    Hall = b.ClassSession.Hall,
                    BookedAt = b.BookedAt,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }
    }
}