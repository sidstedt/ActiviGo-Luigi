using ActiviGo.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class LocationCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        public ICollection<int> ZoneIds { get; set; } = new List<int>();
    }

    public class LocationUpdateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        public ICollection<Zone> Zones { get; set; }
    }

    public class LocationResponseDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        public ICollection<ZoneResponseDto> Zones { get; set; } = new List<ZoneResponseDto>();
    }
}
