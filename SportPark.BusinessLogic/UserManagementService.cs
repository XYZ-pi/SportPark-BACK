using Microsoft.EntityFrameworkCore;
using SportPark.DataAccess.Context;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;

namespace SportPark.BusinessLogic
{
    public class UserManagementService
    {
        private readonly AppDbContext _context;

        public UserManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponse>> GetByRole(UserRole? role)
        {
            var query = _context.Users.AsQueryable();

            if (role.HasValue)
                query = query.Where(u => u.Role == role.Value);

            return await query
                .OrderBy(u => u.Name)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role.ToString(),
                    RegisteredAt = u.RegisteredAt
                })
                .ToListAsync();
        }

        public async Task<UserResponse?> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                RegisteredAt = user.RegisteredAt
            };
        }
    }
}