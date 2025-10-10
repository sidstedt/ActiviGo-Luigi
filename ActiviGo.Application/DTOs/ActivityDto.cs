using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class ActivityCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public bool IsPrivate { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public int CategoryId { get; set; }
        public int ZoneId { get; set; }
        public Guid? StaffId { get; set; }
    }

    public class ActivityUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public int ZoneId { get; set; }
        public Guid? StaffId { get; set; }
    }

    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPrivate { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public Guid? StaffId { get; set; }
        public string? StaffName { get; set; }
    }
}
