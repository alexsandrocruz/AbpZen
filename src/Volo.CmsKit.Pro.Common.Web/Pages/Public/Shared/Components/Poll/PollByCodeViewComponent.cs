using System;
using System.Linq;
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
    ScriptFiles = new[] { "/Pages/Public/Shared/Components/Poll/PollByCode.js" },
    RefreshUrl = "/CmsKitProPublicWidgets/PollByCode",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsPollByCode")]
public class PollByCodeViewComponent : AbpViewComponent
{
    protected IPollPublicAppService PollPublicAppService { get; }
    protected IOptions<AbpMvcUiOptions> AbpMvcUiOptions { get; }

    public PollByCodeViewComponent(IPollPublicAppService pollPublicAppService, IOptions<AbpMvcUiOptions> options)
    {
        PollPublicAppService = pollPublicAppService;
        AbpMvcUiOptions = options;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string code, string widgetName = null, string returnUrl = null)
    {
        var poll = await PollPublicAppService.FindByCodeAsync(code);

        if (poll == null)
        {
            return View("~/Pages/Public/Shared/Components/Poll/Empty.cshtml", new PollViewModel
            {
                Code = code,
            });
        }

        if (returnUrl.IsNullOrEmpty())
        {
            returnUrl = HttpContext.Request.Path.ToString();
        }

        var pollVote = await PollPublicAppService.GetResultAsync(poll.Id);
        var isVoted = pollVote.PollResultDetails.Any(p => p.IsSelectedForCurrentUser);
        var loginUrl = $"{AbpMvcUiOptions.Value.LoginUrl}?returnUrl={returnUrl}";
        var viewModel = new PollViewModel
        {
            Id = poll.Id,
            Code = code,
            Name = poll.Name,
            WidgetName = widgetName ?? code,
            Question = poll.Question,
            ShowResultWithoutGivingVote = poll.ShowResultWithoutGivingVote,
            AllowMultipleVote = poll.AllowMultipleVote,
            ShowHoursLeft = poll.ShowHoursLeft,
            ShowVoteCount = poll.ShowVoteCount,
            VoteCount = poll.VoteCount,
            Texts = poll.PollOptions.Select(p => p.Text).ToList(),
            OptionIds = poll.PollOptions.Select(p => p.Id).ToList(),
            IsVoted = isVoted,
            EndDate = poll.EndDate,
            ResultShowingEndDate = poll.ResultShowingEndDate,
            PollVoteCount = pollVote.PollVoteCount,
            PollResultDetails = pollVote.PollResultDetails,
            LoginUrl = loginUrl
        };

        return View("~/Pages/Public/Shared/Components/Poll/Default.cshtml", viewModel);
    }
}