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

namespace Volo.CmsKit.Admin.Faqs;

[RequiresFeature(CmsKitProFeatures.FaqEnable)]
[RequiresGlobalFeature(typeof(FaqFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/faq-section")]
[Authorize(CmsKitProAdminPermissions.Faqs.Default)]
public class FaqSectionAdminController : CmsKitProAdminController, IFaqSectionAdminAppService
{
    protected IFaqSectionAdminAppService FaqSectionAdminAppService { get; }

    public FaqSectionAdminController(IFaqSectionAdminAppService faqSectionAdminAppService)
    {
        FaqSectionAdminAppService = faqSectionAdminAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<FaqSectionDto> GetAsync(Guid id)
    {
        return FaqSectionAdminAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<FaqSectionWithQuestionCountDto>> GetListAsync(FaqSectionListFilterDto input)
    {
        return FaqSectionAdminAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<FaqSectionDto> CreateAsync(CreateFaqSectionDto input)
    {
        return FaqSectionAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    public Task<FaqSectionDto> UpdateAsync(Guid id, UpdateFaqSectionDto input)
    {
        return FaqSectionAdminAppService.UpdateAsync(id,input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return FaqSectionAdminAppService.DeleteAsync(id);
    }

    [HttpGet("groups")]
    public Task<Dictionary<string, FaqGroupInfoDto>> GetGroupsAsync()
    {
        return FaqSectionAdminAppService.GetGroupsAsync();
    }
}
