using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Admin.Polls;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

[Widget(
    ScriptTypes = new[] { typeof(PollsWidgetScriptContributor) },
    RefreshUrl = "/CmsKitProAdminWidgets/Polls",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsPolls")]
public class PollsViewComponent : AbpViewComponent
{
    [BindProperty]
    public PollsViewModel ViewModel { get; set; }

    protected IPollAdminAppService PollAdminAppService { get; }

    public PollsViewComponent(IPollAdminAppService pollPublicAppService)
    {
        PollAdminAppService = pollPublicAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var polls = await PollAdminAppService.GetListAsync(new GetPollListInput());

        var model = new PollsViewModel()
        {
            Widgets = polls
                .Items
                .Select(w => new SelectListItem(w.Name, w.Code))
                .ToList()
        };

        return View("~/Pages/CmsKit/Polls/Polls.cshtml", model);
    }
}