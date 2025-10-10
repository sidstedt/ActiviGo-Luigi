using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Category : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
