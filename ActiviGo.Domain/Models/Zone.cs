using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Zone : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsOutdoor { get; set; } = false;

        //navigation 
        public ICollection<Activity> Activities { get; set; }

        public ICollection<ActivityOccurrence> ActivityOccurrences { get; set; } = new List<ActivityOccurrence>();

        public int LocaitonId { get; set; }

        [Required]
        public Location Location { get; set; }
    }
}
