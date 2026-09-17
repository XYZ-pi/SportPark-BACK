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
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public bool SessionDeducted { get; set; } = false;
    }
}