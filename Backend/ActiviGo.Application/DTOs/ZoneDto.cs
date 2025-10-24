using ActiviGo.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ActiviGo.Application.DTOs
{
    //this class is a create dto
    public class ZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; } = false;

        //navigation
        public int LocationId { get; set; }
        public string LocationName { get; set; } 
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public bool IsActive { get; set; } 

    }

    public class ZoneReadDto
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; } = false;

        //navigation
        //public int LocationId { get; set; }
        public string LocationName { get; set; }    = string.Empty;
        //public int ActivityId { get; set; }
        //public string? ActivityName { get; set; }

        public string EnvironmentMessage { get; set; }
        public bool IsActive { get; set; } 

    }

    public class ZoneUpdateDto
    {

        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; }
        public int LocationId { get; set; }
        public bool IsActive { get; set; }
 
    }

    public class ZoneResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; }
        public bool IsActive { get; set; }

    }

    public class CreateZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsOutdoor { get; set; }
        public int LocationId { get; set; }
        public bool IsActive { get; set; } 

    }

}
