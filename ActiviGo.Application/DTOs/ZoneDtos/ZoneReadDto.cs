using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.DTOs.ZoneDtos
{
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
}
