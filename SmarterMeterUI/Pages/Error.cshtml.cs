using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace SmarterMeterUI.Pages;

/// <summary>
/// Page model for the error page, capturing the current request ID for display.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>
    /// The request ID for the current failed request, used to correlate logs.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Whether the request ID should be shown to the user.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Captures the current request or activity ID for display on the error page.
    /// </summary>
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}