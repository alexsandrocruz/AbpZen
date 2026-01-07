using Sapienza.Zen.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.Zen.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class SapienzaZenPageModel : AbpPageModel
    {
        protected SapienzaZenPageModel()
        {
            LocalizationResourceType = typeof(SapienzaZenResource);
        }
    }
}