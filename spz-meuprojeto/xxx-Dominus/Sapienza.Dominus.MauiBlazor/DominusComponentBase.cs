using Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Dominus.MauiBlazor;

public abstract class DominusComponentBase : AbpComponentBase
{
    protected DominusComponentBase()
    {
        LocalizationResource = typeof(DominusResource);
    }
}
