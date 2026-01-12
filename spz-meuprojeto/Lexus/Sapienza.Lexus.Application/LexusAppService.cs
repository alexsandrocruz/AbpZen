using Sapienza.Lexus.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus
{
    /* Inherit your application services from this class.
     */
    public abstract class LexusAppService : ApplicationService
    {
        protected LexusAppService()
        {
            LocalizationResource = typeof(LexusResource);
        }
    }
}
