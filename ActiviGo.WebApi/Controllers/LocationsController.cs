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
        // Create
        // ---------------------------
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _locationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.GetType().GetProperty("Id")?.GetValue(created) }, created);
        }

        // ---------------------------
        // Read all
        // ---------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllAsync();
            return Ok(locations);
        }

        // ---------------------------
        // Read by Id
        // ---------------------------
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetByIdAsync(id);
            if (location == null)
                return NotFound($"Location with ID {id} not found.");

            return Ok(location);
        }

        // ---------------------------
        // Update
        // ---------------------------
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _locationService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Location with ID {id} not found.");

            return Ok(updated);
        }

        // ---------------------------
        // Delete
        // ---------------------------
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _locationService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Location with ID {id} not found.");

            return NoContent();
        }
    }
}
