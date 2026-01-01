using LeptonXDemoApp.Localization;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp
{
    /* Inherit your application services from this class.
     */
    public abstract class LeptonXDemoAppAppService : ApplicationService
    {
        protected LeptonXDemoAppAppService()
        {
            LocalizationResource = typeof(LeptonXDemoAppResource);
        }
    }
}
