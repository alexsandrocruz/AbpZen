using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Gdpr.Localization;

namespace Volo.Abp.Gdpr.Blazor;

public abstract class AbpGdprComponentBase : AbpComponentBase
{
    protected AbpGdprComponentBase()
    {
        ObjectMapperContext = typeof(AbpGdprBlazorModule);
        LocalizationResource = typeof(AbpGdprResource);
    }
}