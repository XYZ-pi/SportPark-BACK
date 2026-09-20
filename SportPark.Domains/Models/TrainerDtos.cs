using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportPark.Domains.Models
{
    public class TrainerCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
    }

    public class TrainerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
    }

    public class TrainerTodayItemResponse
    {
        public string Type { get; set; } = string.Empty; // "Group" или "Individual"
        public int Id { get; set; } // BookingId для Group, PersonalSessionId для Individual — используй для вызова /complete
        public string ClientName { get; set; } = string.Empty;
        public string? ServiceName { get; set; } // null для индивидуальных
        public string StartTime { get; set; } = string.Empty; // "HH:mm"
        public string? Hall { get; set; }
    }
}

