using Sapienza.Lexus.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.Lexus.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class LexusController : AbpControllerBase
    {
        protected LexusController()
        {
            LocalizationResource = typeof(LexusResource);
        }
    }
}