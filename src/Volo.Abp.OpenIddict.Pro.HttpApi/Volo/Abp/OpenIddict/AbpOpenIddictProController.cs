using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict;

public abstract class AbpOpenIddictProController : AbpControllerBase
{
    protected AbpOpenIddictProController()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }
}
