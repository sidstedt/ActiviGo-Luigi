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
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ---------------------------
        // Create booking
        // ---------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            try
            {
                var booking = await _bookingService.CreateBookingAsync(userId, dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violations: full, duplicate, etc.
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Fallback to avoid leaking server details; logs should capture the stack.
                return StatusCode(500, new { message = "Ett oväntat fel uppstod vid bokning." });
            }
        }

        [HttpGet ("AdminGetBookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookingsAdmin(CancellationToken ct)
        {
            var bookings = await _bookingService.GetAllBookingsAdminAsync(ct);
            return Ok(bookings);
        }

        // ---------------------------
        // Read all bookings for user (or specific user if ID provided)
        // ---------------------------
        [HttpGet("UserGetBookings")]
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> GetAll([FromQuery] Guid? userId, CancellationToken ct)
        {
            // Om ingen userId skickas → hämta från JWT claims (inloggad användare)
            var effectiveUserId = userId ?? GetUserId();

            // Om användaren inte är admin och försöker hämta någon annans bokningar → neka
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && userId.HasValue && userId.Value != effectiveUserId)
            {
                return Forbid("Endast administratörer kan hämta andra användares bokningar.");
            }

            var bookings = await _bookingService.GetAllBookingsAsync(effectiveUserId, ct);
            return Ok(bookings);
        }


        // ---------------------------
        // Read booking by id
        // ---------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var userId = GetUserId();
            var booking = await _bookingService.GetBookingByIdAsync(userId, id, ct);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        // ---------------------------
        // Update booking
        // ---------------------------
        

        // ---------------------------
        // Cancel booking
        // ---------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var userId = GetUserId();
            var result = await _bookingService.CancelBookingAsync(userId, id, ct);

            if (!result)
                return NotFound(new { message = "Booking not found." });

            return NoContent();
        }
    }
}
