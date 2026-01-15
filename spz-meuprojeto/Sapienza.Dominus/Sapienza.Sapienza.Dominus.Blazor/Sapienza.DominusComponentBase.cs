using Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Dominus.Blazor;

public abstract class DominusComponentBase : AbpComponentBase
{
    protected DominusComponentBase()
    {
        LocalizationResource = typeof(DominusResource);
    }
}
