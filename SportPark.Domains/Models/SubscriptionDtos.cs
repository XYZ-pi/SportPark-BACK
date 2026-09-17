using SportPark.Domains.Enums;

namespace SportPark.Domains.Models
{
    public class SubscriptionCreateRequest
    {
        public int UserId { get; set; }
        public SubscriptionType Type { get; set; }
        public int TotalSessions { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SubscriptionResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int RemainingSessions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}