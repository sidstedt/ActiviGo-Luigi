using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Booking : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int ActivityOccurrenceId { get; set; }
        public ActivityOccurrence ActivityOccurrence { get; set; }
        
        [Required]
        public BookingStatus Status { get; set; }
    }
}
