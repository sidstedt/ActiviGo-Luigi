using ActiviGo.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeocodingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GoogleMaps:ApiKey"]
                  ?? throw new ArgumentNullException("GoogleMaps:ApiKey is not set in configuration");
    }

    public async Task<(double Latitude, double Longitude)?> GetCoordinatesFromAddressAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return null;

        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GeocodingResponse>(json);

        if (result == null || result.status != "OK" || result.results.Length == 0)
            return null;

        var location = result.results[0].geometry.location;
        return (location.lat, location.lng);
    }

    private class GeocodingResponse
    {
        public string status { get; set; } = "";
        public GeocodingResult[] results { get; set; } = Array.Empty<GeocodingResult>();
    }

    private class GeocodingResult
    {
        public Geometry geometry { get; set; } = new Geometry();
    }

    private class Geometry
    {
        public Location location { get; set; } = new Location();
    }

    private class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
