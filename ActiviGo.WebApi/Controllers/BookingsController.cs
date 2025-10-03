using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookings;
        public BookingsController(IBookingService bookings)
        {
            _bookings = bookings;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("get-all-bookings")]
        [Authorize(Roles="User")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings(CancellationToken ct)
        {
            var userId = GetUserId();
            var bookings = await _bookings.GetAllBookingsAsync(userId, ct);
            return Ok(bookings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingDto>> GetBookingById(int id, CancellationToken ct)
        {
            var userId = GetUserId();
            var booking = await _bookings.GetBookingByIdAsync(userId, id, ct);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });
            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<CreatedBookingDto>> CreateBooking([FromBody] CreateBookingDto dto, CancellationToken ct)
        {
            var userId = GetUserId();
            var booking = await _bookings.CreateBookingAsync(userId, dto, ct);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpDelete("{bookingId:int}")]
        public async Task<IActionResult> CancelBooking(int bookingId, CancellationToken ct)
        {
            var userId = GetUserId();
            var result = await _bookings.CancelBookingAsync(userId, bookingId, ct);
            if (!result)
                return NotFound(new { message = "Bokning hittades inte." });
            return NoContent();
        }
    }
}
