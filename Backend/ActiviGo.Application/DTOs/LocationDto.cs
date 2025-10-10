using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class LocationCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        // Räknas ut från adressen via Geocoding API
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Lista med befintliga zoners ID:n
        public ICollection<int> ZoneIds { get; set; } = new List<int>();
    }

    public class LocationUpdateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public ICollection<int>? ZoneIds { get; set; }
    }

    public class LocationResponseDto
    {
        public int Id { get; set; }

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
