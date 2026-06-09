using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmarterMeterUI.Pages;

/// <summary>
/// Login page model — validates password and sets session cookie.
/// </summary>
public class LoginModel(IConfiguration config) : PageModel
{
    /// <summary>
    /// Whether the login attempt failed.
    /// </summary>
    public bool Error { get; set; }

    /// <summary>
    /// Handles password submission.
    /// </summary>
    public IActionResult OnPost(string password)
    {
        string? correct = config["Password"];
        if (password == correct)
        {
            HttpContext.Session.SetString("authenticated", "true");
            return RedirectToPage("/Index");
        }

        Error = true;
        return Page();
    }
}