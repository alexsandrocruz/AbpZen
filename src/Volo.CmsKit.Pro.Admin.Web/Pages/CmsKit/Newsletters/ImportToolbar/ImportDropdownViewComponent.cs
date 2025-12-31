using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters.ImportToolbar;

public class ImportDropdownViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Pages/CmsKit/Newsletters/ImportToolbar/Default.cshtml");
    }
}