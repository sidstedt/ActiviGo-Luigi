using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class ActivityOccurence : BaseEntity
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(1, 120)]
        public int DurationMinutes { get; set; }
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public Activity Activity { get; set; }
        [Required]
        public int ZoneId { get; set; }
        [Required]
        public Zone Zone { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
