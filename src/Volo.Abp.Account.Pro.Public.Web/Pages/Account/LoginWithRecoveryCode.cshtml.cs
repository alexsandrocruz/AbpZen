using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Auditing;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

[DisableAuditing]
public class LoginWithRecoveryCode : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [BindProperty]
    [Required]
    public string RecoveryCode { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        // Ensure the user has gone through the username & password screen first
        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            Logger.LogWarning("Unable to load two-factor authentication user.");
            return RedirectToPage("./Login", new
            {
                ReturnUrl = ReturnUrl,
                ReturnUrlHash = ReturnUrlHash
            });
        }

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            Logger.LogWarning("Unable to load two-factor authentication user.");
            return RedirectToPage("./Login", new
            {
                ReturnUrl = ReturnUrl,
                ReturnUrlHash = ReturnUrlHash
            });
        }

        var result =
            await SignInManager.TwoFactorRecoveryCodeSignInAsync(RecoveryCode.Replace(" ", string.Empty));
        if (result.Succeeded)
        {
            Logger.LogInformation("User with ID '{userId}' logged in with a recovery code.", user.Id);
            return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
        }
        if (result.IsLockedOut)
        {
            Logger.LogWarning("User account locked out.");
            return RedirectToPage("./LockedOut", new {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
        }

        Logger.LogWarning("Invalid recovery code entered for user with ID '{userId}' ", user.Id);
        Alerts.Warning("Invalid recovery code entered.");
        return Page();
    }
}
