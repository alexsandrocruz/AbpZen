using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Pro;

[Dependency(ReplaceServices = true)]
public class AbpOpenIddictProBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "AbpOpenIddictPro";
}
