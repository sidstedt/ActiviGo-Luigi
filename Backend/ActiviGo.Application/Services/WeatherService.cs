using System.Globalization;
using System.Text.Json;
using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ActiviGo.Application.Services
{
    // SMHI point forecast implementation for IWeatherService (ca ~10 days)
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherService> _logger;
        private readonly string? _userAgent;

        public WeatherService(HttpClient httpClient, IMemoryCache cache, ILogger<WeatherService> logger, IConfiguration config)
        {
            _http = httpClient;
            _cache = cache;
            _logger = logger;
            _userAgent = config["Smhi:UserAgent"]; // e.g., "ActiviGo/1.0 (+contact@example.com)"
        }

        private static string MakeCacheKey(double lat, double lon, DateTime at)
            => $"smhi:{Math.Round(lat, 3)}:{Math.Round(lon, 3)}:{at.ToUniversalTime():yyyyMMddHH}";

        public async Task<WeatherDataDto?> GetForecastAsync(WeatherQueryDto query, CancellationToken ct = default)
        {
            // SMHI provides ~10 days. Guard early to avoid unnecessary calls.
            var nowUtc = DateTime.UtcNow;
            if (query.At.ToUniversalTime() > nowUtc.AddDays(10))
            {
                return null;
            }

            var key = MakeCacheKey(query.Latitude, query.Longitude, query.At);
            if (_cache.TryGetValue<WeatherDataDto>(key, out var cached))
                return cached;

            try
            {
                // Build SMHI point forecast URL
                // API: https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{lon}/lat/{lat}/data.json
                var latStr = query.Latitude.ToString(CultureInfo.InvariantCulture);
                var lonStr = query.Longitude.ToString(CultureInfo.InvariantCulture);
                var url = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{lonStr}/lat/{latStr}/data.json";

                // SMHI asks for a descriptive User-Agent
                if (!string.IsNullOrWhiteSpace(_userAgent))
                {
                    if (_http.DefaultRequestHeaders.UserAgent.ToString() != _userAgent)
                    {
                        _http.DefaultRequestHeaders.UserAgent.Clear();
                        _http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", _userAgent);
                    }
                }

                using var resp = await _http.GetAsync(url, ct);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("SMHI responded {Status}", resp.StatusCode);
                    return null;
                }

                var json = await resp.Content.ReadAsStringAsync(ct);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (!root.TryGetProperty("timeSeries", out var seriesArr) || seriesArr.ValueKind != JsonValueKind.Array)
                {
                    return null;
                }

                var atUtc = query.At.ToUniversalTime();
                int bestIndex = -1;
                double bestMinutes = double.MaxValue;
                for (int i = 0; i < seriesArr.GetArrayLength(); i++)
                {
                    var item = seriesArr[i];
                    if (item.TryGetProperty("validTime", out var vtEl))
                    {
                        var vtStr = vtEl.GetString();
                        if (DateTimeOffset.TryParse(vtStr, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dto))
                        {
                            var t = dto.UtcDateTime;
                            var diff = Math.Abs((t - atUtc).TotalMinutes);
                            if (diff < bestMinutes)
                            {
                                bestMinutes = diff;
                                bestIndex = i;
                            }
                        }
                    }
                }

                if (bestIndex < 0)
                    return null;

                var best = seriesArr[bestIndex];
                if (!best.TryGetProperty("parameters", out var paramsArr) || paramsArr.ValueKind != JsonValueKind.Array)
                    return null;

                double? tempC = null;
                double? windMs = null;
                int? wsymb2 = null;

                for (int i = 0; i < paramsArr.GetArrayLength(); i++)
                {
                    var p = paramsArr[i];
                    if (!p.TryGetProperty("name", out var nameEl) || nameEl.ValueKind != JsonValueKind.String) continue;
                    var name = nameEl.GetString();
                    if (!p.TryGetProperty("values", out var vals) || vals.ValueKind != JsonValueKind.Array || vals.GetArrayLength() == 0) continue;

                    switch (name)
                    {
                        case "t": // Celsius
                            if (vals[0].ValueKind == JsonValueKind.Number)
                                tempC = vals[0].GetDouble();
                            break;
                        case "ws": // m/s
                            if (vals[0].ValueKind == JsonValueKind.Number)
                                windMs = vals[0].GetDouble();
                            break;
                        case "Wsymb2": // weather symbol code 1..27
                            if (vals[0].ValueKind == JsonValueKind.Number)
                                wsymb2 = vals[0].GetInt32();
                            break;
                    }
                }

                var data = new WeatherDataDto
                {
                    Temperature = tempC.HasValue ? (int?)Math.Round(tempC.Value, 0, MidpointRounding.AwayFromZero) : null,
                    WindMs = windMs.HasValue ? (int?)Math.Round(windMs.Value, 0, MidpointRounding.AwayFromZero) : null,
                    Icon = wsymb2.HasValue ? MapSmhiIcon(wsymb2.Value) : null,
                    Summary = wsymb2.HasValue ? MapSmhiSummary(wsymb2.Value) : null
                };

                _cache.Set(key, data, TimeSpan.FromHours(2));
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMHI weather fetch failed");
                return null;
            }
        }

        public async Task<IReadOnlyList<WeatherResultDto>> GetForecastBatchAsync(IReadOnlyList<WeatherQueryDto> queries, CancellationToken ct = default)
        {
            var results = new List<WeatherResultDto>(queries.Count);
            foreach (var q in queries)
            {
                WeatherDataDto? forecast = null;
                string? error = null;
                try
                {
                    forecast = await GetForecastAsync(q, ct);
                    if (forecast == null)
                    {
                        if (q.At.ToUniversalTime() > DateTime.UtcNow.AddDays(10))
                            error = "Prognos tillgänglig upp till ca 10 dagar.";
                        else
                            error = "Ingen prognos tillgänglig.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SMHI batch item failed");
                    error = "Fel vid prognoshämtning.";
                }

                results.Add(new WeatherResultDto
                {
                    OccurrenceId = q.OccurrenceId,
                    Forecast = forecast,
                    Error = error
                });
            }
            return results;
        }

        private static string MapSmhiIcon(int code) => code switch
        {
            1 => "clear-day",
            2 or 3 or 4 => "partly-cloudy",
            5 or 6 or 7 => "cloudy",
            8 => "drizzle",  // light rain showers
            9 or 10 => "rain", // moderate/heavy rain showers
            11 => "thunderstorm",
            12 or 13 or 14 => "rain", // sleet showers (closest: rain)
            15 or 16 or 17 => "snow", // snow showers
            18 => "drizzle", // light rain
            19 or 20 => "rain", // moderate/heavy rain
            21 => "thunderstorm",
            22 or 23 or 24 => "rain", // sleet
            25 or 26 or 27 => "snow",
            _ => "unknown"
        };

        private static string MapSmhiSummary(int code) => code switch
        {
            1 => "Klart",
            2 or 3 or 4 => "Mest klart",
            5 or 6 => "Mulet",
            7 => "Dis",
            8 => "Lätta skurar",
            9 => "Måttliga skurar",
            10 => "Kraftiga skurar",
            11 => "Åska",
            12 => "Snöblandade skurar",
            13 => "Snöblandade skurar",
            14 => "Snöblandade skurar",
            15 => "Snöbyar",
            16 => "Snöbyar",
            17 => "Snöbyar",
            18 => "Lätt regn",
            19 => "Måttligt regn",
            20 => "Kraftigt regn",
            21 => "Åska",
            22 => "Snöblandat regn",
            23 => "Snöblandat regn",
            24 => "Snöblandat regn",
            25 => "Lätt snö",
            26 => "Måttlig snö",
            27 => "Kraftig snö",
            _ => "Okänt väder"
        };
    }
}
