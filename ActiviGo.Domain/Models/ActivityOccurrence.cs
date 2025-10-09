using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class ActivityOccurrence : BaseEntity
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        [Range(1, 120)]
        public int DurationMinutes { get; set; }
        [Required]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        [Required]
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
