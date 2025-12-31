using Volo.Forms.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Forms.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class FormsPageModel : AbpPageModel
{
    protected FormsPageModel()
    {
        LocalizationResourceType = typeof(FormsResource);
        ObjectMapperContext = typeof(FormsWebModule);
    }
}
