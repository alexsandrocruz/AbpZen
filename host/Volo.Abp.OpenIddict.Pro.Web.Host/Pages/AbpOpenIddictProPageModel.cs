using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Pro.Pages;

public abstract class AbpOpenIddictProPageModel : AbpPageModel
{
    protected AbpOpenIddictProPageModel()
    {
        LocalizationResourceType = typeof(AbpOpenIddictResource);
    }
}
