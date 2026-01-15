using Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.Dominus.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class DominusController : AbpControllerBase
    {
        protected DominusController()
        {
            LocalizationResource = typeof(DominusResource);
        }
    }
}