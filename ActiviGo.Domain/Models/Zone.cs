using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Zone : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public ZoneType InOut { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
