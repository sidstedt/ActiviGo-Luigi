using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> CreateOccurrence(
            [FromBody] ActivityOccurrenceCreateDto createDto)
        {
            try
            {
                var response = await _occurrenceService.CreateOccurrenceAsync(createDto);
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ICollection<ActivityOccurrenceResponseDto>>> GetAllOccurrences()
        {
            var occurrences = await _occurrenceService.GetAllOccurrencesAsync();
            return Ok(occurrences);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> GetOccurrenceById(int id)
        {
            var occurrence = await _occurrenceService.GetOccurrenceByIdAsync(id);
            if (occurrence == null)
            {
                return NotFound();
            }
            return Ok(occurrence);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ActivityOccurrenceResponseDto>> UpdateOccurrence(
            int id,
            [FromBody] ActivityOccurrenceUpdateDto updateDto)
        {
            try
            {
                var response = await _occurrenceService.UpdateOccurrenceAsync(id, updateDto);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOccurrence(int id)
        {
            var success = await _occurrenceService.DeleteOccurrenceAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}