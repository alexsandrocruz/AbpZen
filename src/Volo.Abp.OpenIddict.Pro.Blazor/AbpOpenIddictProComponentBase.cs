using Volo.Abp.AspNetCore.Components;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Pro.Blazor;

public abstract class AbpOpenIddictProComponentBase : AbpComponentBase
{
    protected AbpOpenIddictProComponentBase()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }
}
