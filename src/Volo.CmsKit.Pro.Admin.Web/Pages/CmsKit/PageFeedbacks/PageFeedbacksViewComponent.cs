using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

[ViewComponent(Name = "CmsPageFeedbacks")]
public class PageFeedbacksViewComponent : AbpViewComponent
{
    protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; }
    protected IStringLocalizer<CmsKitResource> L { get; }

    public PageFeedbacksViewComponent(IPageFeedbackAdminAppService pageFeedbackAdminAppService, IStringLocalizer<CmsKitResource> l)
    {
        PageFeedbackAdminAppService = pageFeedbackAdminAppService;
        L = l;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var entityTypes = await PageFeedbackAdminAppService.GetEntityTypesAsync();
        entityTypes.AddFirst("");
        return View("~/Pages/CmsKit/PageFeedbacks/PageFeedbacks.cshtml", new PageFeedbackViewDto{
            EntityTypes = entityTypes.Select(x => new SelectListItem(L[x], x)).ToList()
        });
    }
}