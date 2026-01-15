using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.Sapienza.Dominus.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class Sapienza.DominusPageModel : AbpPageModel
    {
        protected Sapienza.DominusPageModel()
        {
            LocalizationResourceType = typeof(Sapienza.DominusResource);
        }
    }
}