using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportPark.Domains.Models
{
    public class BookingCreateRequest
    {
        public int ClassSessionId { get; set; }
    }

    public class BookingResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string Hall { get; set; } = string.Empty;
        public DateTime BookedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}