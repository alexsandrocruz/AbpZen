using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Polls;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Poll;

[Widget(
    ScriptTypes = new[] { typeof(PollWidgetScriptContributor) },
    ScriptFiles = new[] { "/Pages/Public/Shared/Components/Poll/Poll.js" },
    RefreshUrl = "/CmsKitProPublicWidgets/Poll",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsPoll")]
public class PollViewComponent : AbpViewComponent
{
    protected IPollPublicAppService PollPublicAppService { get; }
    protected IOptions<AbpMvcUiOptions> AbpMvcUiOptions { get; }

    public PollViewComponent(IPollPublicAppService pollPublicAppService, IOptions<AbpMvcUiOptions> options)
    {
        PollPublicAppService = pollPublicAppService;
        AbpMvcUiOptions = options;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string widgetName)
    {
        var isWidgetNameAvailable = await PollPublicAppService.IsWidgetNameAvailableAsync(widgetName);

        if (!isWidgetNameAvailable)
        {
            return View("~/Pages/Public/Shared/Components/Poll/Empty.cshtml", new PollViewModel
            {
                Code = widgetName
            });
        }
        
        var poll = await PollPublicAppService.FindByAvailableWidgetAsync(widgetName);

        if (poll == null)
        {
            return Content(string.Empty);
        }

        if (!CheckDateIntervals(poll))
        {
            return View("~/Pages/Public/Shared/Components/Poll/Default.cshtml", new PollViewModel
            {
                Id = null
            });
        }

        var pollViewComponent = new PollByCodeViewComponent(PollPublicAppService, AbpMvcUiOptions);

        return await pollViewComponent.InvokeAsync(poll.Code, widgetName, HttpContext.Request.Path.ToString());
    }

    private bool CheckDateIntervals(PollWithDetailsDto poll)
    {
        var now = DateTime.Now;
        if (poll.StartDate > now)
        {
            return false;
        }

        if (poll.ResultShowingEndDate.HasValue && poll.ResultShowingEndDate.Value < now)
        {
            return false;
        }

        return true;
    }
}