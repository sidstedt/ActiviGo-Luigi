using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Domain.Models
{
    public class Location : BaseEntity
    {
        [Required(ErrorMessage = "Namn är obligatoriskt.")]
        [MaxLength(100, ErrorMessage = "Namn får inte vara längre än 100 tecken.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adress är obligatorisk.")]
        [MaxLength(200, ErrorMessage = "Adress får inte vara längre än 200 tecken.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Latitud är obligatorisk.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitud är obligatorisk.")]
        public double Longitude { get; set; }

        public ICollection<Zone> Zones { get; set; } = new List<Zone>();
    }
}
