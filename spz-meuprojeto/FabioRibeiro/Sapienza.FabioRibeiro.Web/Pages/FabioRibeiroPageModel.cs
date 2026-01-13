using Sapienza.FabioRibeiro.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.FabioRibeiro.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class FabioRibeiroPageModel : AbpPageModel
    {
        protected FabioRibeiroPageModel()
        {
            LocalizationResourceType = typeof(FabioRibeiroResource);
        }
    }
}