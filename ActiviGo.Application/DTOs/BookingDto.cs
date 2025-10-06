using ActiviGo.Domain.Enum;

namespace ActiviGo.Application.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ActivityOccurrenceId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public decimal ActivityPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }
    }

    public class CreateBookingDto
    {
        public int ActivityOccurrenceId { get; set; }
    }

    public class CreatedBookingDto
    {
        public int Id { get; set; }
        public int ActivityOccurrenceId { get; set; }
        public int ActivityId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class UpdateBookingDto
    {
        public BookingStatus Status { get; set; }
    }
}
