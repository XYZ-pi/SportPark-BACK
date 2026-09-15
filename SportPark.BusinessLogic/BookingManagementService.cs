using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SportPark.BusinessLogic
{
    public class BookingManagementService
    {
        private readonly AppDbContext _context;

        public BookingManagementService(AppDbContext context)
        {
            _context = context;
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

            var booking = new Booking
            {
                UserId = userId,
                ClassSessionId = request.ClassSessionId,
                BookedAt = DateTime.UtcNow,
                Status = BookingStatus.Confirmed
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

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
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null) return false;

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
