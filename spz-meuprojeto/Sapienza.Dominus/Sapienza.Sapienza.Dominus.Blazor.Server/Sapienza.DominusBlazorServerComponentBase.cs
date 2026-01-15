using Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Dominus.Blazor
{
    public abstract class DominusBlazorServerComponentBase : AbpComponentBase
    {
        protected DominusBlazorServerComponentBase()
        {
            LocalizationResource = typeof(DominusResource);
        }
    }
}
