using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs.ZoneDtos
{
    public class ZoneCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]

        //navigation
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
    }
}
