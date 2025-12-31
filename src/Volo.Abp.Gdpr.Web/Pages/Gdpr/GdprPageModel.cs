using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Gdpr.Localization;

namespace Volo.Abp.Gdpr.Web.Pages.Gdpr;

public abstract class GdprPageModel : AbpPageModel
{
    public GdprPageModel()
    {
        ObjectMapperContext = typeof(AbpGdprWebModule);
        LocalizationResourceType = typeof(AbpGdprResource);
    }
}