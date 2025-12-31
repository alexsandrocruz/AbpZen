using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Faqs;

[RequiresFeature(CmsKitProFeatures.FaqEnable)]
[RequiresGlobalFeature(typeof(FaqFeature))]
[RemoteService(Name = CmsKitProCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/faq-section")]
public class FaqSectionPublicController : CmsKitProCommonController, IFaqSectionPublicAppService
{
    protected IFaqSectionPublicAppService FaqSectionPublicAppService { get; }

    public FaqSectionPublicController(IFaqSectionPublicAppService faqSectionPublicAppService)
    {
        FaqSectionPublicAppService = faqSectionPublicAppService;
    }

    [HttpGet]
    public Task<List<FaqSectionWithQuestionsDto>> GetListSectionWithQuestionsAsync(FaqSectionListFilterInput input)
    {
        return FaqSectionPublicAppService.GetListSectionWithQuestionsAsync(input);
    }
}
