using Sapienza.FabioRibeiro.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.FabioRibeiro
{
    /* Inherit your application services from this class.
     */
    public abstract class FabioRibeiroAppService : ApplicationService
    {
        protected FabioRibeiroAppService()
        {
            LocalizationResource = typeof(FabioRibeiroResource);
        }
    }
}
