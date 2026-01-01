using LeptonXDemoApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LeptonXDemoApp.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class LeptonXDemoAppController : AbpControllerBase
    {
        protected LeptonXDemoAppController()
        {
            LocalizationResource = typeof(LeptonXDemoAppResource);
        }
    }
}