using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Public.Web.Pages;

public abstract class CmsKitProPublicPageModel : AbpPageModel
{
    public CmsKitProPublicPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitProPublicWebModule);
    }
}
