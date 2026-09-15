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
    public class ContactRequestManagementService
    {
        private readonly AppDbContext _context;

        public ContactRequestManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequestResponse> Create(ContactRequestCreateRequest request)
        {
            var contactRequest = new ContactRequest
            {
                Name = request.Name,
                Phone = request.Phone,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactRequests.Add(contactRequest);
            await _context.SaveChangesAsync();

            return new ContactRequestResponse
            {
                Id = contactRequest.Id,
                Name = contactRequest.Name,
                Phone = contactRequest.Phone,
                Message = contactRequest.Message,
                CreatedAt = contactRequest.CreatedAt
            };
        }

        public async Task<List<ContactRequestResponse>> GetAll()
        {
            return await _context.ContactRequests
                .OrderByDescending(cr => cr.CreatedAt)
                .Select(cr => new ContactRequestResponse
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    Phone = cr.Phone,
                    Message = cr.Message,
                    CreatedAt = cr.CreatedAt
                })
                .ToListAsync();
        }
    }
}

