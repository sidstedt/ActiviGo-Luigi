using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // ---------------------------
        // CREATE
        // ---------------------------
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto dto)
        {
            var created = await _locationService.CreateAsync(dto);

            if (created == null)
            {
                return BadRequest("Could not create entity.");
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ---------------------------
        // READ ALL
        // ---------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllAsync();
            return Ok(locations);
        }

        // ---------------------------
        // READ BY ID
        // ---------------------------
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // ---------------------------
        // UPDATE
        // ---------------------------
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationUpdateDto dto)
        {
            var updated = await _locationService.UpdateAsync(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"Location with ID {id} not found." });
            }

            return Ok(updated);
        }

        // ---------------------------
        // DELETE
        // ---------------------------
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _locationService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Location with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
