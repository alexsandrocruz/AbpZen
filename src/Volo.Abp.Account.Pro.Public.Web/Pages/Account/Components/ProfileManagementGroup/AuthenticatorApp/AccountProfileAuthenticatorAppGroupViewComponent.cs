using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.AuthenticatorApp;

public class AccountProfileAuthenticatorAppGroupViewComponent : AbpViewComponent
{
    protected IAccountAppService AccountAppService;

    public AccountProfileAuthenticatorAppGroupViewComponent(IAccountAppService accountAppService)
    {
        AccountAppService = accountAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var authenticatorInfo = await AccountAppService.GetAuthenticatorInfoAsync();
        var model = new AuthenticatorAppModel
        {
            HasAuthenticator = await AccountAppService.HasAuthenticatorAsync(),
            SharedKey = authenticatorInfo.Key,
            AuthenticatorUri = authenticatorInfo.Uri
        };

        return View("~/Pages/Account/Components/ProfileManagementGroup/AuthenticatorApp/Default.cshtml", model);
    }

    public class AuthenticatorAppModel
    {
        public bool HasAuthenticator { get; set; }

        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        public string Code { get; set; }
    }
}
