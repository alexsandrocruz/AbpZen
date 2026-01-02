using LeptonXDemoApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace LeptonXDemoApp.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class LeptonXDemoAppPageModel : AbpPageModel
    {
        protected LeptonXDemoAppPageModel()
        {
            LocalizationResourceType = typeof(LeptonXDemoAppResource);
        }
    }
}