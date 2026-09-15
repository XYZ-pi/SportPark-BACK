using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SportPark.BusinessLogic
{
    public class ClassSessionManagementService
    {
        private readonly AppDbContext _context;

        public ClassSessionManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassSessionResponse>> GetAll()
        {
            return await _context.ClassSessions
                .Include(cs => cs.Service)
                .Include(cs => cs.Trainer)
                    .ThenInclude(t => t!.User)
                .Select(cs => new ClassSessionResponse
                {
                    Id = cs.Id,
                    ServiceName = cs.Service!.Name,
                    TrainerName = cs.Trainer!.User!.Name,
                    DayOfWeek = cs.DayOfWeek,
                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                    Hall = cs.Hall
                })
                .ToListAsync();
        }

        public async Task<List<ClassSessionResponse>> GetByDay(DayOfWeek day)
        {
            return await _context.ClassSessions
                .Include(cs => cs.Service)
                .Include(cs => cs.Trainer)
                    .ThenInclude(t => t!.User)
                .Where(cs => cs.DayOfWeek == day)
                .OrderBy(cs => cs.StartTime)
                .Select(cs => new ClassSessionResponse
                {
                    Id = cs.Id,
                    ServiceName = cs.Service!.Name,
                    TrainerName = cs.Trainer!.User!.Name,
                    DayOfWeek = cs.DayOfWeek,
                    StartTime = cs.StartTime.ToString(@"hh\:mm"),
                    Hall = cs.Hall
                })
                .ToListAsync();
        }

        public async Task<ClassSessionResponse> Create(ClassSessionCreateRequest request)
        {
            var serviceExists = await _context.Services.AnyAsync(s => s.Id == request.ServiceId);
            if (!serviceExists)
                throw new InvalidOperationException("Услуга с таким Id не найдена");

            var trainerExists = await _context.Trainers.AnyAsync(t => t.Id == request.TrainerId);
            if (!trainerExists)
                throw new InvalidOperationException("Тренер с таким Id не найден");

            if (!TimeSpan.TryParse(request.StartTime, out var parsedTime))
                throw new InvalidOperationException("Неверный формат времени, ожидается HH:mm");

            var session = new ClassSession
            {
                ServiceId = request.ServiceId,
                TrainerId = request.TrainerId,
                DayOfWeek = request.DayOfWeek,
                StartTime = parsedTime,
                Hall = request.Hall
            };

            _context.ClassSessions.Add(session);
            await _context.SaveChangesAsync();

            var service = await _context.Services.FindAsync(request.ServiceId);
            var trainer = await _context.Trainers.Include(t => t.User).FirstAsync(t => t.Id == request.TrainerId);

            return new ClassSessionResponse
            {
                Id = session.Id,
                ServiceName = service!.Name,
                TrainerName = trainer.User!.Name,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime.ToString(@"hh\:mm"),
                Hall = session.Hall
            };
        }

        public async Task<bool> Delete(int id)
        {
            var session = await _context.ClassSessions.FindAsync(id);
            if (session == null) return false;

            _context.ClassSessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}