using Sapienza.Zen.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.Zen.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class SapienzaZenController : AbpControllerBase
    {
        protected SapienzaZenController()
        {
            LocalizationResource = typeof(SapienzaZenResource);
        }
    }
}