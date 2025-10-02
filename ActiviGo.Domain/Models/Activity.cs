using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Activity : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required, Range(typeof(decimal), "0", "500.00")]
        public decimal Price { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required, Range(typeof(int), "1", "50")]
        public int MaxParticipants { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }

        [Required]
        public Guid? StaffId { get; set; }
        public User? Staff { get; set; }

        public ICollection<ActivityOccurence> Occurences { get; set; } = new List<ActivityOccurence>();
    }
}
