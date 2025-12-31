using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[RequiresFeature(CmsKitProFeatures.PageFeedbackEnable)]
[RequiresGlobalFeature(typeof(PageFeedbackFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/page-feedback")]
[Authorize(CmsKitProAdminPermissions.PageFeedbacks.Default)]
public class PageFeedbackAdminController : CmsKitProAdminController, IPageFeedbackAdminAppService
{
    protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; }

    public PageFeedbackAdminController(IPageFeedbackAdminAppService pageFeedbackAdminAppService)
    {
        PageFeedbackAdminAppService = pageFeedbackAdminAppService;
    }
    
    [HttpGet]
    [Route("{id}")]
    public Task<PageFeedbackDto> GetAsync(Guid id)
    {
        return PageFeedbackAdminAppService.GetAsync(id);
    }
    
    [HttpGet]
    public Task<PagedResultDto<PageFeedbackDto>> GetListAsync(GetPageFeedbackListInput input)
    {
        return PageFeedbackAdminAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Update)]
    public Task<PageFeedbackDto> UpdateAsync(Guid id, UpdatePageFeedbackDto dto)
    {
        return PageFeedbackAdminAppService.UpdateAsync(id, dto);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return PageFeedbackAdminAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("entity-types")]
    public Task<List<string>> GetEntityTypesAsync()
    {
        return PageFeedbackAdminAppService.GetEntityTypesAsync();
    }

    [HttpGet]
    [Route("settings")]
    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Settings)]
    public Task<List<PageFeedbackSettingDto>> GetSettingsAsync()
    {
        return PageFeedbackAdminAppService.GetSettingsAsync();
    }

    [HttpPut]
    [Route("settings")]
    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Settings)]
    public Task UpdateSettingsAsync(UpdatePageFeedbackSettingsInput input)
    {
        return PageFeedbackAdminAppService.UpdateSettingsAsync(input);
    }
}