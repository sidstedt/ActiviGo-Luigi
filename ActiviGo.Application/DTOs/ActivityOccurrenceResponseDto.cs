using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class ActivityOccurrenceResponseDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxCapacity { get; set; }
        public int ParticipantsCount { get; set; }
        public int AvailableSlots { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty; 
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public bool IsActive { get; set; }
    }
}
