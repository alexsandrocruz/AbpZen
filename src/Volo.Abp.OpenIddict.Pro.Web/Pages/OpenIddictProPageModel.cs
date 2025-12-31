using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Pro.Web.Pages;

public abstract class OpenIddictProPageModel : AbpPageModel
{
    protected OpenIddictProPageModel()
    {
        LocalizationResourceType = typeof(AbpOpenIddictResource);
        ObjectMapperContext = typeof(AbpOpenIddictProWebModule);
    }
}
