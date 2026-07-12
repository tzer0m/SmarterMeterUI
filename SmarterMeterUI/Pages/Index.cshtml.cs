using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmarterMeterUI.Models;
using SmarterMeterUI.Services;

namespace SmarterMeterUI.Pages;

/// <summary>
/// Index page model — fetches meter readings and summary data from the API.
/// </summary>
/// <param name="meterService">The meter service used to fetch data.</param>
[Authorize]
public class IndexModel(MeterService meterService) : PageModel
{
    /// <summary>
    /// All readings fetched from the API, used for chart rendering.
    /// </summary>
    public List<MeterReading> Readings { get; set; } = [];

    /// <summary>
    /// The most recent reading.
    /// </summary>
    public MeterReading? LatestReading { get; set; }

    /// <summary>
    /// Pre-calculated usage and cost summary for today, last 7 days, and last 30 days.
    /// </summary>
    public MeterSummary Summary { get; set; } = new();

    /// <summary>
    /// Loads the meter summary and readings for display.
    /// </summary>
    public async Task OnGetAsync()
    {
        Summary = await meterService.GetSummaryAsync();
        Readings = await meterService.GetReadingsAsync(5000);
        Readings = [.. Readings.OrderBy(r => r.CapturedAt)];
        LatestReading = Readings.LastOrDefault();
    }
}