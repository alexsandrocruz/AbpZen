using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users.ExportToolbar;

public class ExportDropdownViewComponent: AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Pages/Identity/Users/ExportToolbar/Default.cshtml");
    }
}