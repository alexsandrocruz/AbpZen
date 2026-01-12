using Sapienza.Zen.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Zen.MauiBlazor;

public abstract class SapienzaZenComponentBase : AbpComponentBase
{
    protected SapienzaZenComponentBase()
    {
        LocalizationResource = typeof(SapienzaZenResource);
    }
}
