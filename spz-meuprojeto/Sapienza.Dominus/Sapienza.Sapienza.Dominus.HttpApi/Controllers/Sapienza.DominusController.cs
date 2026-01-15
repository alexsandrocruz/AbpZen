using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.Sapienza.Dominus.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class Sapienza.DominusController : AbpControllerBase
    {
        protected Sapienza.DominusController()
        {
            LocalizationResource = typeof(Sapienza.DominusResource);
        }
    }
}