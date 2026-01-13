using Sapienza.EventoZen.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.EventoZen.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class EventoZenPageModel : AbpPageModel
    {
        protected EventoZenPageModel()
        {
            LocalizationResourceType = typeof(EventoZenResource);
        }
    }
}