using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class PasswordResetLinkSentModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public bool PreventEmailEnumeration { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        PreventEmailEnumeration = await SettingProvider.IsTrueAsync(AccountSettingNames.PreventEmailEnumeration);
        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
