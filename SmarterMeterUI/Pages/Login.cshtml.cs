using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmarterMeterUI.Pages;

/// <summary>
/// Login page model — validates password and signs in with a persistent cookie.
/// </summary>
public class LoginModel(IConfiguration config) : PageModel
{
    /// <summary>
    /// Whether the login attempt failed.
    /// </summary>
    public bool Error { get; set; }

    /// <summary>
    /// If already authenticated, skip the login form and go straight to the index page.
    /// </summary>
    public IActionResult OnGet()
    {
        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            return RedirectToPage("/Index");

        return Page();
    }

    /// <summary>
    /// Handles password submission.
    /// </summary>
    public async Task<IActionResult> OnPost(string password)
    {
        string? correct = config["Password"];
        if (password == correct)
        {
            // Build a minimal identity - no roles or user-specific claims needed, just proof of a successful login
            ClaimsIdentity identity = new([new Claim(ClaimTypes.Name, "SmarterMeterUser")], CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);

            // IsPersistent + the cookie's own ExpireTimeSpan (set in Program.cs) is what makes this survive browser restarts
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });
            return RedirectToPage("/Index");
        }

        Error = true;
        return Page();
    }
}