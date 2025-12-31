using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Volo.Abp.OpenIddict.Pro.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class AbpOpenIddictProBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OpenIddictPro";
}
