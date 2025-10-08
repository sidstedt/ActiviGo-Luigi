using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    //create dto
    public class ZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; } = false;

        //navigation
        public int LocationId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
    }

    public class ZoneReadDto
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; } = false;

        //navigation
        public int LocationId { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
    }

    public class ZoneUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; }
        public int LocationId { get; set; }

        //navigation
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
    }

    public class ZoneResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; }
    }

}
