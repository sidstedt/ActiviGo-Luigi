using ActiviGo.Application.DTOs;

namespace ActiviGo.Application.Interfaces
{
    public interface IWeatherService
    {
        // Returns null if outside supported window or on provider failure (up to implementation)
        Task<WeatherDataDto?> GetForecastAsync(WeatherQueryDto query, CancellationToken ct = default);

        // Batch version for efficiency. Always returns one result per query input.
        Task<IReadOnlyList<WeatherResultDto>> GetForecastBatchAsync(IReadOnlyList<WeatherQueryDto> queries, CancellationToken ct = default);
    }
}
