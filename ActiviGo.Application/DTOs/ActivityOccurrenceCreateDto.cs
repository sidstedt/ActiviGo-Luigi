using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.DTOs
{
    public class ActivityOccurrenceCreateDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Range(1, 120)]
        public int? DurationMinutes { get; set; }
        [Required]
        public int ActivityId { get; set; }
        public int? ZoneId { get; set; }
    }
}
