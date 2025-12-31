using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Pages;

public abstract class ProPageModel : AbpPageModel
{
    protected ProPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
    }
}
