using Sapienza.Lexus.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Lexus.Blazor
{
    public abstract class LexusComponentBase : AbpComponentBase
    {
        protected LexusComponentBase()
        {
            LocalizationResource = typeof(LexusResource);
        }
    }
}
