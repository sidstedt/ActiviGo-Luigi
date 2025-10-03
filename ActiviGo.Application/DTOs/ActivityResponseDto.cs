using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
        public int StandardDurationMinutes { get; set; }
        public bool IsOutdoor { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public string? StaffId { get; set; }
        public string? StaffName { get; set; }
    }
}
