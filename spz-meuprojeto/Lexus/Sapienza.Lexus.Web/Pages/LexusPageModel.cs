using Sapienza.Lexus.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.Lexus.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class LexusPageModel : AbpPageModel
    {
        protected LexusPageModel()
        {
            LocalizationResourceType = typeof(LexusResource);
        }
    }
}