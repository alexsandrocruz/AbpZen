using Sapienza.Zen.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Zen.Blazor;

public abstract class SapienzaZenComponentBase : AbpComponentBase
{
    protected SapienzaZenComponentBase()
    {
        LocalizationResource = typeof(SapienzaZenResource);
    }
}
