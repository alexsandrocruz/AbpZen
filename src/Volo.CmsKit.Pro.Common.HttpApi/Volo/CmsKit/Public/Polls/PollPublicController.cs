using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Polls;

[RequiresFeature(CmsKitProFeatures.PollEnable)]
[RequiresGlobalFeature(typeof(PollsFeature))]
[RemoteService(Name = CmsKitProCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/poll")]
public class PollPublicController : CmsKitProCommonController, IPollPublicAppService
{
    protected IPollPublicAppService PollPublicAppService { get; }

    public PollPublicController(IPollPublicAppService pollPublicAppService)
    {
        PollPublicAppService = pollPublicAppService;
    }

    [HttpGet]
    [Route("widget-name-available")]
    public Task<bool> IsWidgetNameAvailableAsync(string widgetName)
    {
        return PollPublicAppService.IsWidgetNameAvailableAsync(widgetName);
    }

    [HttpGet]
    [Route("by-available-widget-name")]
    public virtual async Task<PollWithDetailsDto> FindByAvailableWidgetAsync(string widgetName)
    {
        return await PollPublicAppService.FindByAvailableWidgetAsync(widgetName);
    }

    [HttpGet]
    [Route("by-code")]
    public virtual async Task<PollWithDetailsDto> FindByCodeAsync(string code)
    {
        return await PollPublicAppService.FindByCodeAsync(code);
    }

    [HttpGet]
    [Route("result/{id}")]
    public virtual async Task<GetResultDto> GetResultAsync(Guid id)
    {
        return await PollPublicAppService.GetResultAsync(id);
    }

    [HttpPost]
    [Route("{id}")]
    public virtual async Task SubmitVoteAsync(Guid id, SubmitPollInput submitPollInput)
    {
        await PollPublicAppService.SubmitVoteAsync(id, submitPollInput);
    }
}
