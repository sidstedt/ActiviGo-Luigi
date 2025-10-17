using System.ComponentModel.DataAnnotations;

namespace ActiviGo.Application.DTOs
{
    // Client -> API query for a weather forecast
    public class WeatherQueryDto
    {
        // Optional: to correlate response with an occurrence in the UI
        public int? OccurrenceId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        // Point-in-time to get forecast for (UTC recommended)
        [Required]
        public DateTime At { get; set; }
    }

    // Weather data returned to clients
    public class WeatherDataDto
    {
        // Short textual description (e.g., "Lätt regn")
        public string? Summary { get; set; }

        // Icon key per provider mapping (e.g., "rain", "cloudy", "clear-day")
        public string? Icon { get; set; }

        // Temperatur i Celsius (avrundad, heltal)
        public int? Temperature { get; set; }

        // Vindhastighet i m/s (avrundad, heltal)
        public int? WindMs { get; set; }
    }

    // API response item for each query
    public class WeatherResultDto
    {
        public int? OccurrenceId { get; set; }

        // Null if outside supported window (>5 days), missing coordinates, or provider failure
        public WeatherDataDto? Forecast { get; set; }

        // Optional error message for degraded responses
        public string? Error { get; set; }
    }
}
