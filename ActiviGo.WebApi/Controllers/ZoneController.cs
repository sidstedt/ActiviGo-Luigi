using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Models;
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
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ZoneResponseDto>>> GetAllZonesWithActivitesAndLocation()
        {
            var zones = await _zone.GetAllZonesWithRelations();
            return Ok(zones);
        }

        //get api/zone/location/{locationId}
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
        [HttpGet]
        [Route("{id:int}/activities")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ZoneResponseDto>>> GetZonesWithActivitiesById(int id)
        {
            var zones = await _zone.GetZonesWithActivititesByIdAsync(id);
            return Ok(zones);
        }

        //add activity to zone
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
        [HttpDelete]
        [Route("{zoneId:int}/activities/{activityId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveActivityFromZone(int zoneId, int activityId)
        {
            await _zone.RemoveActivityFromZone(zoneId, activityId);
            return Ok("Removed Successfully");
        }
        
    }
}
