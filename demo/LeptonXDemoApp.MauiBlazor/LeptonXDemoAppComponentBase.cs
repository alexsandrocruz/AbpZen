using LeptonXDemoApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace LeptonXDemoApp.MauiBlazor;

public abstract class LeptonXDemoAppComponentBase : AbpComponentBase
{
    protected LeptonXDemoAppComponentBase()
    {
        LocalizationResource = typeof(LeptonXDemoAppResource);
    }
}
