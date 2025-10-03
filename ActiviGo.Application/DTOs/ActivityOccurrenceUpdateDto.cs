using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class ActivityOccurrenceUpdateDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        [Range(1, 120)]
        public int DurationMinutes { get; set; }
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public int ZoneId { get; set; }
        public bool IsCancelled { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
