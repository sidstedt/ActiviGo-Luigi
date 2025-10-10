using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IZoneService _zone;

        public ZoneController(IZoneService zone)
        {
            _zone = zone;
        }

        //get api/zone/withRelations
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ZoneResponseDto>>> GetAllZonesWithActivitesAndLocation()
        {
            var zones = await _zone.GetAllZonesWithRelations();
            return Ok(zones);
        }

        //get api/zone/location/{locationId}
        [AllowAnonymous]
        [HttpGet]
        [Route("location/{locationId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ZoneDto>>> GetZonesByLocationId(int locationId)
        {
            var zones = await _zone.GetZonesByLocationId(locationId);
            return Ok(zones);
        }

        //get api/zone/{id}/activities
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:int}/activities")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ZoneResponseDto>>> GetZonesWithActivitiesById(int id)
        {
            var zones = await _zone.GetZonesWithActivititesByIdAsync(id);
            return Ok(zones);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _zone.CreateAsync(dto);
            return Ok(created);

        }

        //add activity to zone
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("{zoneId:int}/activities/{activityId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddActivityToZone(int zoneId, int activityId)
        {
            await _zone.AddActivityToZone(zoneId, activityId);
            return Ok("Added Successfully");
        }

        //delete api/zone/{zoneId}/activities/{activityId}
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{zoneId:int}/activities/{activityId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveActivityFromZone(int zoneId, int activityId)
        {
            await _zone.RemoveActivityFromZone(zoneId, activityId);
            return Ok("Removed Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteZone(int id)
        {
            await _zone.DeleteAsync(id);
            return Ok($"Deleted Successfully fro id {id}");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateZone(int id, [FromBody] ZoneUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _zone.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { Message = $"Activity with Id {id} not found." });

            return Ok(updated);
        }
    }
}
