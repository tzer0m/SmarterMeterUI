using SmarterMeterUI.Models;
using System.Text.Json;

namespace SmarterMeterUI.Services;

/// <summary>
/// Fetches meter readings from the SmarterMeter API.
/// </summary>
public class MeterService(HttpClient httpClient, IConfiguration config)
{
    /// <summary>
    /// Api base url
    /// </summary>
    private readonly string ApiBaseUrl = config["SmarterMeter:ApiBaseUrl"] ?? throw new InvalidOperationException("SmarterMeter:ApiBaseUrl is not configured");

    /// <summary>
    /// Api key
    /// </summary>
    private readonly string ApiKey = config["SmarterMeter:ApiKey"] ?? throw new InvalidOperationException("SmarterMeter:ApiKey is not configured");

    /// <summary>
    /// Json options
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Fetches the most recent meter readings from the API.
    /// </summary>
    /// <param name="count">Number of readings to fetch.</param>
    public async Task<List<MeterReading>> GetReadingsAsync(int count = 200)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, $"{ApiBaseUrl}/smartermeter/readings?count={count}");
        request.Headers.Add("X-API-Key", ApiKey);

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<MeterReading>>(json, JsonOptions) ?? [];
    }
}