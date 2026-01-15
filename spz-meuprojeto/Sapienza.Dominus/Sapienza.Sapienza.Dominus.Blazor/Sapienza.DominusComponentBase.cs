using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Sapienza.Dominus.Blazor;

public abstract class Sapienza.DominusComponentBase : AbpComponentBase
{
    protected Sapienza.DominusComponentBase()
    {
        LocalizationResource = typeof(Sapienza.DominusResource);
    }
}
