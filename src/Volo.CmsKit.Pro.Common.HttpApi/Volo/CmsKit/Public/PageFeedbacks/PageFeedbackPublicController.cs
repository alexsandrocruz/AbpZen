using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.PageFeedbacks;

[RequiresFeature(CmsKitProFeatures.PageFeedbackEnable)]
[RequiresGlobalFeature(typeof(PageFeedbackFeature))]
[RemoteService(Name = CmsKitProCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/page-feedback")]
public class PageFeedbackPublicController : CmsKitProCommonController, IPageFeedbackPublicAppService
{
    protected virtual IPageFeedbackPublicAppService PageFeedbackPublicAppService { get; }

    public PageFeedbackPublicController(IPageFeedbackPublicAppService pageFeedbackPublicAppService)
    {
        PageFeedbackPublicAppService = pageFeedbackPublicAppService;
    }

    [HttpPost]
    public Task<PageFeedbackDto> CreateAsync(CreatePageFeedbackInput input)
    {
        return PageFeedbackPublicAppService.CreateAsync(input);
    }

    [HttpPost]
    [Route("initialize-user-note")]
    public Task<PageFeedbackDto> InitializeUserNoteAsync(InitializeUserNoteInput input)
    {
        return PageFeedbackPublicAppService.InitializeUserNoteAsync(input);
    }

    [HttpPost]
    [Route("change-is-useful")]
    public Task<PageFeedbackDto> ChangeIsUsefulAsync(ChangeIsUsefulInput input)
    {
        return PageFeedbackPublicAppService.ChangeIsUsefulAsync(input);
    }
}
