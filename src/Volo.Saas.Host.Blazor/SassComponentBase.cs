using Volo.Abp.AspNetCore.Components;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Blazor;

public abstract class SassComponentBase : AbpComponentBase
{
    protected SassComponentBase()
    {
        LocalizationResource = typeof(SaasResource);
        ObjectMapperContext = typeof(SaasHostBlazorModule);
    }
}
