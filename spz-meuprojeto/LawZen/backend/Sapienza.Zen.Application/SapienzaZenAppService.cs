using Sapienza.Zen.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.Zen
{
    /* Inherit your application services from this class.
     */
    public abstract class SapienzaZenAppService : ApplicationService
    {
        protected SapienzaZenAppService()
        {
            LocalizationResource = typeof(SapienzaZenResource);
        }
    }
}
