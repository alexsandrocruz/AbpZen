using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

namespace Volo.CmsKit.Pro.Admin.Web.Controllers;

public class CmsKitProAdminWidgetsController : AbpController
{
    public IActionResult Polls()
    {
        return ViewComponent(typeof(PollsViewComponent));
    }
}

