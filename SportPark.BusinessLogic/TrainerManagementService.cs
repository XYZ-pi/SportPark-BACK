using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Enums;
using SportPark.Domains.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SportPark.BusinessLogic
{
    public class TrainerManagementService
    {
        private readonly AppDbContext _context;

        public TrainerManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainerResponse>> GetAll()
        {
            return await _context.Trainers
                .Include(t => t.User)
                .Select(t => new TrainerResponse
                {
                    Id = t.Id,
                    Name = t.User!.Name,
                    Email = t.User.Email,
                    Specialization = t.Specialization,
                    Bio = t.Bio,
                    PhotoUrl = t.PhotoUrl
                })
                .ToListAsync();
        }

        public async Task<TrainerResponse?> GetById(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null) return null;

            return new TrainerResponse
            {
                Id = trainer.Id,
                Name = trainer.User!.Name,
                Email = trainer.User.Email,
                Specialization = trainer.Specialization,
                Bio = trainer.Bio,
                PhotoUrl = trainer.PhotoUrl
            };
        }

        public async Task<TrainerResponse> Create(TrainerCreateRequest request)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existing != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                Role = UserRole.Trainer,
                RegisteredAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var trainer = new Trainer
            {
                UserId = user.Id,
                Specialization = request.Specialization,
                Bio = request.Bio,
                PhotoUrl = request.PhotoUrl
            };
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            return new TrainerResponse
            {
                Id = trainer.Id,
                Name = user.Name,
                Email = user.Email,
                Specialization = trainer.Specialization,
                Bio = trainer.Bio,
                PhotoUrl = trainer.PhotoUrl
            };
        }

        public async Task<bool> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return false;

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}