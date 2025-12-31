using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Public.Polls;

[RequiresFeature(CmsKitProFeatures.PollEnable)]
[RequiresGlobalFeature(PollsFeature.Name)]

public class PollPublicAppService : PublicAppService, IPollPublicAppService
{
    private readonly CmsKitPollingOptions _cmsKitPollingOptions;
    protected IPollRepository PollRepository { get; }
    protected IPollUserVoteRepository PollUserVoteRepository { get; }
    protected PollManager PollManager { get; }

    public PollPublicAppService(
        IPollRepository pollRepository,
        IPollUserVoteRepository pollUserVoteRepository,
        PollManager pollManager, IOptions<CmsKitPollingOptions> cmsKitPollingOptions)
    {
        PollRepository = pollRepository;
        PollUserVoteRepository = pollUserVoteRepository;
        PollManager = pollManager;
        _cmsKitPollingOptions = cmsKitPollingOptions.Value;
    }

    public Task<bool> IsWidgetNameAvailableAsync(string widgetName)
    {
        return Task.FromResult(_cmsKitPollingOptions.WidgetNames.Contains(widgetName));
    }

    public virtual async Task<PollWithDetailsDto> FindByAvailableWidgetAsync(string widgetName)
    {
        var poll = await PollRepository.FindByAvailableWidgetAsync(widgetName, Clock.Now);
        if (poll == null)
        {
            return null;
        }

        poll.OrderPollOptions();

        return ObjectMapper.Map<Poll, PollWithDetailsDto>(poll);
    }

    public virtual async Task<PollWithDetailsDto> FindByCodeAsync(string code)
    {
        var poll = await PollRepository.FindByCodeAsync(code);
        if (poll == null)
        {
            return null;
        }

        poll.OrderPollOptions();

        return ObjectMapper.Map<Poll, PollWithDetailsDto>(poll);
    }

    public virtual async Task<GetResultDto> GetResultAsync(Guid id)
    {
        var pollWithVotes = (await PollRepository.GetPollWithPollUserVotesAsync(id)).First();

        var resultDetails = new List<PollResultDto>();

        var poll = pollWithVotes.Key;
        var pollUserVotes = pollWithVotes.Value;

        var userVotes = CurrentUser.IsAuthenticated
            ? pollUserVotes.Where(v => v.UserId == CurrentUser.GetId()).Select(v => v.PollOptionId).ToList()
            : new List<Guid>();

        poll.OrderPollOptions();
        foreach (var pollOption in poll.PollOptions)
        {
            resultDetails.Add(new PollResultDto()
            {
                Text = pollOption.Text,
                VoteCount = pollOption.VoteCount,
                IsSelectedForCurrentUser = userVotes.Contains(pollOption.Id)
            });
        }

        return new GetResultDto
        {
            PollVoteCount = poll.VoteCount,
            Question = poll.Question,
            PollResultDetails = resultDetails
        };
    }

    [Authorize]
    public virtual async Task SubmitVoteAsync(Guid id, SubmitPollInput submitPollInput)
    {
        await PollManager.SubmitVoteAsync(id, CurrentUser.GetId(), submitPollInput.PollOptionIds);
    }
}
