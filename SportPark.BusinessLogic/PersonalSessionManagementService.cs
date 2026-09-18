using Microsoft.EntityFrameworkCore;
using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;

namespace SportPark.BusinessLogic
{
    public class PersonalSessionManagementService
    {
        private readonly AppDbContext _context;
        private readonly SubscriptionManagementService _subscriptionService;

        public PersonalSessionManagementService(AppDbContext context, SubscriptionManagementService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public async Task<PersonalSessionResponse> Create(int trainerUserId, PersonalSessionCreateRequest request)
        {
            var trainer = await _context.Trainers.Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == trainerUserId);
            if (trainer == null)
                throw new InvalidOperationException("Профиль тренера не найден");

            var client = await _context.Users.FindAsync(request.ClientId);
            if (client == null)
                throw new InvalidOperationException("Клиент не найден");

            var subscription = await _subscriptionService.GetActiveSubscription(request.ClientId, SubscriptionType.Individual);
            if (subscription == null)
                throw new InvalidOperationException("У клиента нет активного индивидуального абонемента с оставшимися посещениями");

            var session = new PersonalSession
            {
                TrainerId = trainer.Id,
                ClientId = request.ClientId,
                SessionStart = request.SessionStart,
                DurationMinutes = request.DurationMinutes
            };

            _context.PersonalSessions.Add(session);
            await _context.SaveChangesAsync();

            return MapToResponse(session, trainer.User!.Name, client.Name);
        }

        public async Task<List<PersonalSessionResponse>> GetForTrainer(int trainerUserId)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == trainerUserId);
            if (trainer == null) return new List<PersonalSessionResponse>();

            return await _context.PersonalSessions
                .Include(ps => ps.Client)
                .Include(ps => ps.Trainer).ThenInclude(t => t!.User)
                .Where(ps => ps.TrainerId == trainer.Id)
                .Select(ps => new PersonalSessionResponse
                {
                    Id = ps.Id,
                    ClientName = ps.Client!.Name,
                    TrainerName = ps.Trainer!.User!.Name,
                    SessionStart = ps.SessionStart,
                    DurationMinutes = ps.DurationMinutes,
                    Completed = ps.Completed,
                    Cancelled = ps.Cancelled
                })
                .ToListAsync();
        }

        public async Task<List<PersonalSessionResponse>> GetForClient(int clientUserId)
        {
            return await _context.PersonalSessions
                .Include(ps => ps.Client)
                .Include(ps => ps.Trainer).ThenInclude(t => t!.User)
                .Where(ps => ps.ClientId == clientUserId)
                .Select(ps => new PersonalSessionResponse
                {
                    Id = ps.Id,
                    ClientName = ps.Client!.Name,
                    TrainerName = ps.Trainer!.User!.Name,
                    SessionStart = ps.SessionStart,
                    DurationMinutes = ps.DurationMinutes,
                    Completed = ps.Completed,
                    Cancelled = ps.Cancelled
                })
                .ToListAsync();
        }

        public async Task<bool> Cancel(int clientUserId, int id)
        {
            var session = await _context.PersonalSessions
                .FirstOrDefaultAsync(ps => ps.Id == id && ps.ClientId == clientUserId);

            if (session == null || session.Completed || session.Cancelled) return false;

            var isLateCancel = (session.SessionStart - DateTime.Now).TotalHours < 2;

            session.Cancelled = true;

            if (isLateCancel)
            {
                await DeductSession(clientUserId);
                session.SessionDeducted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkCompleted(int requestingUserId, bool isAdmin, int id)
        {
            var session = await _context.PersonalSessions.FirstOrDefaultAsync(ps => ps.Id == id);
            if (session == null || session.Completed || session.Cancelled) return false;

            if (!isAdmin)
            {
                var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == requestingUserId);
                if (trainer == null || trainer.Id != session.TrainerId) return false;
                if (session.SessionStart.Date != DateTime.Now.Date) return false;
            }

            session.Completed = true;
            session.SessionDeducted = true;
            await DeductSession(session.ClientId);

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task DeductSession(int clientUserId)
        {
            var subscription = await _subscriptionService.GetActiveSubscription(clientUserId, SubscriptionType.Individual);
            if (subscription != null)
            {
                subscription.RemainingSessions -= 1;
                if (subscription.RemainingSessions <= 0)
                    subscription.IsActive = false;
            }
        }

        private static PersonalSessionResponse MapToResponse(PersonalSession s, string trainerName, string clientName)
        {
            return new PersonalSessionResponse
            {
                Id = s.Id,
                ClientName = clientName,
                TrainerName = trainerName,
                SessionStart = s.SessionStart,
                DurationMinutes = s.DurationMinutes,
                Completed = s.Completed,
                Cancelled = s.Cancelled
            };
        }
    }
}