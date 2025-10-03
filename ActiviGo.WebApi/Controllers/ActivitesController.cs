using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ActiviGo.Application.Interfaces;
using ActiviGo.Application.DTOs;
using System.Collections.Generic;
using System;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitesController(IActivityService activityService)
        {
            _activityService = activityService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityCreateDto activityDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newActivity = await _activityService.CreateActivityAsync(activityDto);

                return CreatedAtAction(nameof(GetActivityById), new { id = newActivity.Id }, newActivity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internt fel vid skapande.", Error = ex.Message });
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ICollection<ActivityResponseDto>>> GetAllActivities()
        {
            {
                var activities = await _activityService.GetAllActivitiesAsync();
                return Ok(activities);
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ActivityResponseDto>> GetActivityById(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound(new { Message = $"Aktivitet med Id {id} hittades inte." });
            }
            return Ok(activity);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateActivity(int id, [FromBody] ActivityUpdateDto activityDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedActivity = await _activityService.UpdateActivityAsync(id, activityDto);
                return Ok(updatedActivity);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                await _activityService.DeleteActivityAsync(id);
                return NoContent();
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
    }
}
