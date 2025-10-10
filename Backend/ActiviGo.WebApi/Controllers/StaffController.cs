using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class StaffController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IActivityOccurrenceService _occurrenceService;
        private readonly IBookingService _bookingService;

        public StaffController(
            IActivityService activityService,
            IActivityOccurrenceService occurrenceService,
            IBookingService bookingService)
        {
            _activityService = activityService;
            _occurrenceService = occurrenceService;
            _bookingService = bookingService;
        }

        // GET: api/staff/activities
        [HttpGet("activities")]
        public async Task<IActionResult> GetMyActivities(CancellationToken ct)
        {
            var staffId = GetCurrentUserId();
            var activities = await _activityService.GetByStaffAsync(staffId, ct);
            return Ok(activities);
        }

        // GET: api/staff/occurrences?from=2025-10-01&to=2025-10-31
        [HttpGet("occurrences")]
        public async Task<IActionResult> GetMyOccurrences([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
        {
            var staffId = GetCurrentUserId();
            var occurrences = await _occurrenceService.GetByStaffAsync(staffId, from, to, ct);
            return Ok(occurrences);
        }

        // GET: api/staff/occurrences/{id}/bookings
        [HttpGet("occurrences/{id:int}/bookings")]
        public async Task<IActionResult> GetBookingsForOccurrence(int id, CancellationToken ct)
        {
            var staffId = GetCurrentUserId();
            // Service should validate that occurrence.Activity.StaffId == staffId
            var bookings = await _bookingService.GetBookingsForOccurrenceAsync(id, staffId, ct);
            if (bookings == null) return NotFound(new { Message = $"Occurrence {id} not found or not owned by this staff member." });
            return Ok(bookings);
        }

        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null) throw new UnauthorizedAccessException("Missing user identifier claim.");
            return Guid.Parse(claim);
        }
    }
}
