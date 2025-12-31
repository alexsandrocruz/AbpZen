using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.AuthorityDelegation;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
　
namespace Volo.Abp.Account.Pro.Public.Web.Shared;

public class AbpAccountPublicWebSharedBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAccountAuthorityDelegationOptions>>();

        if (options.Value.EnableDelegatedImpersonation)
        {
            context.Files.Add("/Pages/Account/AuthorityDelegation/account-authority-delegation-global.js");
        }

        context.Files.Add("/Pages/Account/LinkUsers/account-link-user-global.js");
    }
}
