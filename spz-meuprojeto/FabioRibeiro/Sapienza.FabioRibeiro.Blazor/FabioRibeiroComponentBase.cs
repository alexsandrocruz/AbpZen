using Sapienza.FabioRibeiro.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.FabioRibeiro.Blazor;

public abstract class FabioRibeiroComponentBase : AbpComponentBase
{
    protected FabioRibeiroComponentBase()
    {
        LocalizationResource = typeof(FabioRibeiroResource);
    }
}
