using Sapienza.Dominus.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.Dominus.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class DominusPageModel : AbpPageModel
    {
        protected DominusPageModel()
        {
            LocalizationResourceType = typeof(DominusResource);
        }
    }
}