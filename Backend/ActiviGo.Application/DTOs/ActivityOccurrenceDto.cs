using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    public class ActivityOccurrenceCreateDto
    {
        public DateTime StartTime { get; set; }

        public int? DurationMinutes { get; set; }

        public int ActivityId { get; set; }

        public int? ZoneId { get; set; }
    }

    public class ActivityOccurrenceUpdateDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int DurationMinutes { get; set; }

        public int ActivityId { get; set; }

        public int ZoneId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ActivityOccurrenceResponseDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxCapacity { get; set; }
        public int ParticipantsCount { get; set; }
        public int AvailableSlots { get; set; }

        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;

        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsOutdoor { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsActive { get; set; }
    }
}
