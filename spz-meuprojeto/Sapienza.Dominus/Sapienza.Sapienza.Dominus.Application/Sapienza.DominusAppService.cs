using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.Sapienza.Dominus
{
    /* Inherit your application services from this class.
     */
    public abstract class Sapienza.DominusAppService : ApplicationService
    {
        protected Sapienza.DominusAppService()
        {
            LocalizationResource = typeof(Sapienza.DominusResource);
        }
    }
}
