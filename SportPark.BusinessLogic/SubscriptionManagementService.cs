using Microsoft.EntityFrameworkCore;
using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;

namespace SportPark.BusinessLogic
{
    public class SubscriptionManagementService
    {
        private readonly AppDbContext _context;

        public SubscriptionManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionResponse> Create(SubscriptionCreateRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                throw new InvalidOperationException("Клиент не найден");

            var subscription = new Subscription
            {
                UserId = request.UserId,
                Type = request.Type,
                TotalSessions = request.TotalSessions,
                RemainingSessions = request.TotalSessions,
                StartDate = DateTime.UtcNow,
                EndDate = request.EndDate,
                IsActive = true
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return MapToResponse(subscription, user.Name);
        }

        public async Task<List<SubscriptionResponse>> GetForUser(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .Select(s => new SubscriptionResponse
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    ClientName = s.User!.Name,
                    Type = s.Type.ToString(),
                    TotalSessions = s.TotalSessions,
                    RemainingSessions = s.RemainingSessions,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }

        // Используется внутри BookingManagementService и PersonalSessionManagementService для списания
        public async Task<Subscription?> GetActiveSubscription(int userId, SubscriptionType type)
        {
            return await _context.Subscriptions
                .Where(s => s.UserId == userId && s.Type == type && s.IsActive && s.RemainingSessions > 0)
                .OrderBy(s => s.EndDate) // сначала списываем с того, что заканчивается раньше
                .FirstOrDefaultAsync();
        }

        private static SubscriptionResponse MapToResponse(Subscription s, string clientName)
        {
            return new SubscriptionResponse
            {
                Id = s.Id,
                UserId = s.UserId,
                ClientName = clientName,
                Type = s.Type.ToString(),
                TotalSessions = s.TotalSessions,
                RemainingSessions = s.RemainingSessions,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsActive = s.IsActive
            };
        }
    }
}