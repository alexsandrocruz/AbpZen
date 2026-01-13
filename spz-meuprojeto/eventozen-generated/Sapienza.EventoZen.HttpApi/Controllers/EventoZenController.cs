using Sapienza.EventoZen.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.EventoZen.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class EventoZenController : AbpControllerBase
    {
        protected EventoZenController()
        {
            LocalizationResource = typeof(EventoZenResource);
        }
    }
}