using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Pages.Saas.Host;

public abstract class SaasHostPageModel : AbpPageModel
{
    public SaasHostPageModel()
    {
        LocalizationResourceType = typeof(SaasResource);
        ObjectMapperContext = typeof(SaasHostWebModule);
    }
}
