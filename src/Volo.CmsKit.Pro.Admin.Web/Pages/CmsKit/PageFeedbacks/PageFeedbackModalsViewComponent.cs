using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Admin.PageFeedbacks;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

[ViewComponent(Name = "CmsPageFeedbackModals")]
public class PageFeedbackModalsViewComponent : AbpViewComponent
{
    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Pages/CmsKit/PageFeedbacks/PageFeedbackModals.cshtml", new PageFeedbackModalViewDto());
    }
}