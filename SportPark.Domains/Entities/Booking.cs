using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportPark.Domains.Enums;

namespace SportPark.Domains.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ClassSessionId { get; set; }
        public ClassSession? ClassSession { get; set; }
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }
}
