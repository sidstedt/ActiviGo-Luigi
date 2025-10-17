using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // Batch endpoint: accepts multiple queries and returns one result per query.
        [HttpPost("forecast")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<WeatherResultDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetForecastBatch([FromBody] IReadOnlyList<WeatherQueryDto> queries, CancellationToken ct)
        {
            if (queries == null || queries.Count == 0)
                return BadRequest("Queries cannot be empty.");

            foreach (var q in queries)
            {
                if (q.At == default)
                    return BadRequest("Each query must include a valid 'At' timestamp.");
            }

            var result = await _weatherService.GetForecastBatchAsync(queries, ct);
            return Ok(result);
        }

    }
}
