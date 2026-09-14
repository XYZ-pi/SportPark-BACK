using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportPark.Domains.Entities
{
    public class ClassSession
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
        public DayOfWeek DayOfWeek { get; set; } // встроенный enum .NET, свой писать не надо
        public TimeSpan StartTime { get; set; }
        public string Hall { get; set; } = string.Empty;
    }
}
