using Volo.Forms.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Forms.Pages;

public abstract class FormsPageModel : AbpPageModel
{
    protected FormsPageModel()
    {
        LocalizationResourceType = typeof(FormsResource);
    }
}
