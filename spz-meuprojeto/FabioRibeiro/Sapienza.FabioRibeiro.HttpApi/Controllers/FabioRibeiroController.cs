using Sapienza.FabioRibeiro.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.FabioRibeiro.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class FabioRibeiroController : AbpControllerBase
    {
        protected FabioRibeiroController()
        {
            LocalizationResource = typeof(FabioRibeiroResource);
        }
    }
}