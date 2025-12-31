using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class ExternalPasswordLoginModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [Required]
    public string UserNameOrEmailAddress { get; set; }

    [BindProperty]
    [DisableAuditing]
    [Required]
    public string Password { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await IdentityOptions.SetAsync();
        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return RedirectToPage("./Login");
        }

        var user = await this.UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        if (user == null)
        {
            Logger.LogWarning("Could not find user with external login info");
            return RedirectToPage("./Login");
        }

        UserNameOrEmailAddress = user.Email;
        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await IdentityOptions.SetAsync();
        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");
            return RedirectToPage("./Login");
        }

        var user = await this.UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        if (user == null)
        {
            Logger.LogWarning("Could not find user with external login info");
            return RedirectToPage("./Login");
        }

        var result = await SignInManager.CheckPasswordSignInAsync(user, Password, true);
        if (result.Succeeded)
        {
            var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (auth.Succeeded)
            {
                auth.Properties.Items.TryAdd(nameof(ExternalPasswordLoginModel), nameof(ExternalPasswordLoginModel));
                await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, auth.Principal, auth.Properties);
            }

            var url = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
            return Redirect(url ?? "./Login");
        }

        Alerts.Danger(L["InvalidUserNameOrPassword"]);

        UserNameOrEmailAddress = user.Email;
        return Page();
    }
}
