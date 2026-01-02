using LeptonXDemoApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace LeptonXDemoApp.Blazor
{
    public abstract class LeptonXDemoAppComponentBase : AbpComponentBase
    {
        protected LeptonXDemoAppComponentBase()
        {
            LocalizationResource = typeof(LeptonXDemoAppResource);
        }
    }
}
