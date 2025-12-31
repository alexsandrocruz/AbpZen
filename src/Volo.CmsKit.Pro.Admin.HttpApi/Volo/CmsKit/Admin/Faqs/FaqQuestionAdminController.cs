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
[Route("api/cms-kit-admin/faq-question")]
[Authorize(CmsKitProAdminPermissions.Faqs.Default)]
public class FaqQuestionAdminController : CmsKitProAdminController, IFaqQuestionAdminAppService
{
    protected IFaqQuestionAdminAppService FaqQuestionAdminAppService { get; }

    public FaqQuestionAdminController(IFaqQuestionAdminAppService faqQuestionAdminAppService)
    {
        FaqQuestionAdminAppService = faqQuestionAdminAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<FaqQuestionDto> GetAsync(Guid id)
    {
        return FaqQuestionAdminAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<FaqQuestionDto>> GetListAsync(FaqQuestionListFilterDto input)
    {
        return FaqQuestionAdminAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<FaqQuestionDto> CreateAsync(CreateFaqQuestionDto input)
    {
        return FaqQuestionAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    public Task<FaqQuestionDto> UpdateAsync(Guid id, UpdateFaqQuestionDto input)
    {
        return FaqQuestionAdminAppService.UpdateAsync(id,input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return FaqQuestionAdminAppService.DeleteAsync(id);
    }
}
