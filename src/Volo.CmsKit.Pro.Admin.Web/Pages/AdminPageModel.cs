using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Admin.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class AdminPageModel : AbpPageModel
{
    public AdminPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitProAdminWebModule);
    }
}
