using Sapienza.Dominus.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus
{
    /* Inherit your application services from this class.
     */
    public abstract class DominusAppService : ApplicationService
    {
        protected DominusAppService()
        {
            LocalizationResource = typeof(DominusResource);
        }
    }
}
