namespace SportPark.Domains.Entities
{
    public class PersonalSession
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
        public int ClientId { get; set; }
        public User? Client { get; set; }
        public DateTime SessionStart { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public bool Completed { get; set; } = false;
        public bool Cancelled { get; set; } = false;
        public bool SessionDeducted { get; set; } = false;
    }
}