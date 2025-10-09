using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class ActivityCreateDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        [Required]
        public bool IsOutdoor { get; set; } = false;

        public int StandardDurationMinutes { get; set; } = 60;

        public bool IsActive { get; set; } = true;

        public bool IsAvailable { get; set; } = true;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ZoneId { get; set; } = string.Empty;

        public string? StaffId { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class ActivityUpdateDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        public int StandardDurationMinutes { get; set; }

        public bool IsOutdoor { get; set; }

        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ZoneId { get; set; } = string.Empty;

        public string? StaffId { get; set; }
    }

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

        public string ZoneId { get; set; } = string.Empty;

        public string? ZoneName { get; set; }

        public string? StaffId { get; set; }

        public string? StaffName { get; set; }
    }

    //for listing activities by category with minimal info
    public class ActivityForCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
