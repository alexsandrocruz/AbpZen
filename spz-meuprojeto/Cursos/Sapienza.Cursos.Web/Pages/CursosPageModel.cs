using Sapienza.Cursos.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Sapienza.Cursos.Web.Pages
{
    /* Inherit your Page Model classes from this class.
     */
    public abstract class CursosPageModel : AbpPageModel
    {
        protected CursosPageModel()
        {
            LocalizationResourceType = typeof(CursosResource);
        }
    }
}