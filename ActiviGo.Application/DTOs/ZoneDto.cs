using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class ZoneDto
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

    public class ZoneReadDto
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //navigation
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
    }

    public class ZoneUpdateDto
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
