using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportPark.DataAccess.Context;
using SportPark.Domains.Entities;
using SportPark.Domains.Models;

namespace SportPark.BusinessLogic
{
    public class ServiceManagementService
    {
        private readonly AppDbContext _context;

        public ServiceManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceResponse>> GetAll()
        {
            return await _context.Services
                .Select(s => new ServiceResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Category = s.Category
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse?> GetById(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return null;

            return new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Category = service.Category
            };
        }

        public async Task<ServiceResponse> Create(ServiceCreateRequest request)
        {
            var service = new Service
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Category = service.Category
            };
        }

        public async Task<bool> Update(int id, ServiceCreateRequest request)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            service.Name = request.Name;
            service.Description = request.Description;
            service.Category = request.Category;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}