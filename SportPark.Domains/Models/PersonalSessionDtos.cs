namespace SportPark.Domains.Models
{
    public class PersonalSessionCreateRequest
    {
        public int ClientId { get; set; }
        public DateTime SessionStart { get; set; }
        public int DurationMinutes { get; set; } = 60;
    }

    public class PersonalSessionResponse
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime SessionStart { get; set; }
        public int DurationMinutes { get; set; }
        public bool Completed { get; set; }
        public bool Cancelled { get; set; }
    }
}