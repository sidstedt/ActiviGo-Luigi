using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.WebApi.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityOccurrenceController : ControllerBase
    {
        private readonly IActivityOccurrenceService _occurrenceService;

        public ActivityOccurrenceController(IActivityOccurrenceService occurrenceService)
        {
            _occurrenceService = occurrenceService;
        }

        // POST: api/ActivityOccurrence
        [HttpPost]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> CreateOccurrence(
            [FromBody] ActivityOccurrenceCreateDto createDto)
        {
            try
            {
                var response = await _occurrenceService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetOccurrenceById), new { id = response.Id }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        // GET: api/ActivityOccurrence
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ActivityOccurrenceResponseDto>>> GetAllOccurrences()
        {
            var occurrences = await _occurrenceService.GetAllAsync();
            return Ok(occurrences);
        }

        // GET: api/ActivityOccurrence/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> GetOccurrenceById(int id)
        {
            var occurrence = await _occurrenceService.GetByIdAsync(id);
            if (occurrence == null)
                return NotFound();

            return Ok(occurrence);
        }

        // PUT: api/ActivityOccurrence/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> UpdateOccurrence(
            int id,
            [FromBody] ActivityOccurrenceUpdateDto updateDto)
        {
            try
            {
                var response = await _occurrenceService.UpdateAsync(id, updateDto);
                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        // DELETE: api/ActivityOccurrence/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOccurrence(int id)
        {
            var success = await _occurrenceService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // GET: api/ActivityOccurrence/activity/{activityId}/available
        [HttpGet("activity/{activityId}/available")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ActivityOccurrenceResponseDto>>> GetAvailableOccurrencesByActivityId(int activityId)
        {
            var available = await _occurrenceService.GetAvailableOccurrencesByActivityIdAsync(activityId);
            return Ok(available);
        }
    }
}
