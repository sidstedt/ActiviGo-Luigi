using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class CreateActivityDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(500)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        [Required]
        public bool IsOutdoor { get; set; } = false;
        public int StandardDurationMinutes { get; set; } = 60;
        public bool IsActive { get; set; }  = true;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string ZoneId { get; set; }
        public string? StaffId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
