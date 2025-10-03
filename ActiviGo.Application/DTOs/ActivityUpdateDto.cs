using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class ActivityUpdateDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(500)]
        public string Description { get; set; }
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
        public string ZoneId { get; set; }
        public string? StaffId { get; set; }
    }
}
