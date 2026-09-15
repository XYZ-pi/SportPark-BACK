using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportPark.Domains.Models
{
    public class ClassSessionCreateRequest
    {
        public int ServiceId { get; set; }
        public int TrainerId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty; // формат "HH:mm", например "09:00"
        public string Hall { get; set; } = string.Empty;
    }

    public class ClassSessionResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string Hall { get; set; } = string.Empty;
    }
}
