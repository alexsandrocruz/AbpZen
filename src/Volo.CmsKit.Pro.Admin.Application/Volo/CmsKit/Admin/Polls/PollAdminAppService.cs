using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Admin.Polls;

[RequiresFeature(CmsKitProFeatures.PollEnable)]
[RequiresGlobalFeature(PollsFeature.Name)]
[Authorize(CmsKitProAdminPermissions.Polls.Default)]
public class PollAdminAppService : CmsKitProAdminAppService, IPollAdminAppService
{
    private readonly CmsKitPollingOptions _cmsKitPollingOptions;
    protected IPollRepository PollRepository { get; }
    protected PollManager PollManager { get; }


    public PollAdminAppService(
        IPollRepository pollRepository,
        IOptions<CmsKitPollingOptions> pollingOptions,
        PollManager pollManager)
    {
        _cmsKitPollingOptions = pollingOptions.Value;
        PollRepository = pollRepository;
        PollManager = pollManager;
    }

    public virtual async Task<PollWithDetailsDto> GetAsync(Guid id)
    {
        var poll = await PollRepository.GetAsync(id);
        poll.OrderPollOptions();
        return ObjectMapper.Map<Poll, PollWithDetailsDto>(poll);
    }

    public virtual async Task<PagedResultDto<PollDto>> GetListAsync(GetPollListInput input)
    {
        var list = await PollRepository.GetListAsync(input.Filter, input.Sorting, input.SkipCount, input.MaxResultCount);

        return new PagedResultDto<PollDto>()
        {
            Items = new List<PollDto>(
                ObjectMapper.Map<List<Poll>, List<PollDto>>(list)
            ),
            TotalCount = await PollRepository.GetCountAsync(input.Filter)
        };
    }

    [Authorize(CmsKitProAdminPermissions.Polls.Create)]
    public virtual async Task<PollWithDetailsDto> CreateAsync(CreatePollDto input)
    {
        
        await CheckDateRangeAvailabilityAsync(input.Widget, input.StartDate, input.EndDate, input.ResultShowingEndDate);

        await PollManager.EnsureExistAsync(input.Code);

        var poll = new Poll(
                GuidGenerator.Create(),
                input.Question,
                input.Code,
                input.Widget,
                input.Name,
                input.StartDate,
                input.AllowMultipleVote,
                input.ShowVoteCount,
                input.ShowResultWithoutGivingVote,
                input.ShowHoursLeft,
                input.EndDate,
                input.ResultShowingEndDate,
                CurrentTenant?.Id);

        input.MapExtraPropertiesTo(poll);

        foreach (var item in input.PollOptions)
        {
            poll.AddPollOption(GuidGenerator.Create(), item.Text, item.Order, CurrentTenant?.Id);
        }

        poll = await PollRepository.InsertAsync(poll);

        return ObjectMapper.Map<Poll, PollWithDetailsDto>(poll);
    }

    [Authorize(CmsKitProAdminPermissions.Polls.Update)]
    public virtual async Task<PollWithDetailsDto> UpdateAsync(Guid id, UpdatePollDto input)
    {
        var poll = await PollRepository.GetAsync(id);
        
        await CheckDateRangeAvailabilityAsync(input.Widget, input.StartDate, input.EndDate, input.ResultShowingEndDate, poll.Code);

        if (poll.Code != input.Code)
        {
            await PollManager.EnsureExistAsync(input.Code);
        }

        poll.SetQuestion(input.Question);
        poll.SetCode(input.Code);
        poll.ShowVoteCount = input.ShowVoteCount;
        poll.ShowResultWithoutGivingVote = input.ShowResultWithoutGivingVote;
        poll.ShowHoursLeft = input.ShowHoursLeft;
        poll.Widget = input.Widget;
        poll.Name = input.Name;
        poll.SetDates(input.StartDate, input.EndDate, input.ResultShowingEndDate);
        input.MapExtraPropertiesTo(poll);
        
        //TODO may make refactor and do these steps in the domain layer
        var removedPollOptionIds = poll.PollOptions.Select(p => p.Id).Except(input.PollOptions.Select(p => p.Id)).ToList();
        
        foreach (var removedPollOptionId in removedPollOptionIds)
        {
            poll.RemovePollOption(removedPollOptionId);
        }

        foreach (var item in input.PollOptions)
        {
            poll.UpdatePollOption(item.Id, item.Text, item.Order, poll.TenantId);
        }

        await PollRepository.UpdateAsync(poll);

        return ObjectMapper.Map<Poll, PollWithDetailsDto>(poll);
    }

    [Authorize(CmsKitProAdminPermissions.Polls.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return PollRepository.DeleteAsync(id);
    }

    public Task<ListResultDto<PollWidgetDto>> GetWidgetsAsync()
    {
        return Task.FromResult(new ListResultDto<PollWidgetDto>()
        {
            Items = _cmsKitPollingOptions.WidgetNames
                .Select(n =>
                    new PollWidgetDto
                    {
                        Name = n
                    }).ToList()
        });
    }

    public virtual async Task<GetResultDto> GetResultAsync(Guid id)
    {
        var poll = await PollRepository.GetAsync(id);
        
        poll.OrderPollOptions();

        var resultDetails = new List<PollResultDto>();

        foreach (var pollOption in poll.PollOptions)
        {
            resultDetails.Add(new PollResultDto()
            {
                Text = pollOption.Text,
                VoteCount = poll.PollOptions.First(p => p.Id == pollOption.Id).VoteCount,
            });
        }

        return new GetResultDto
        {
            PollVoteCount = poll.VoteCount,
            Question = poll.Question,
            PollResultDetails = resultDetails
        };
    }

    private async Task CheckDateRangeAvailabilityAsync(string widget, DateTime startDate, DateTime? endDate = null, DateTime? resultShowingEndDate = null, string code = null)
    {
        if(string.IsNullOrWhiteSpace(widget))
        {
            return;
        }
        
        var poll = await PollRepository.FindByDateRangeAndWidgetAsync(startDate, endDate, resultShowingEndDate, widget);
        if (poll != null && poll.Code != code)
        {
            throw new UserFriendlyException(L["PollWithSameDateRangeAndWidgetAlreadyExists", poll.Code]);
        }
    }
}
