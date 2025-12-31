using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Faqs;

[RequiresFeature(CmsKitProFeatures.FaqEnable)]
[RequiresGlobalFeature(FaqFeature.Name)]
[Authorize(CmsKitProAdminPermissions.Faqs.Default)]
public class FaqQuestionAdminAppService : CmsKitProAdminAppService, IFaqQuestionAdminAppService
{
    protected IFaqQuestionRepository FaqQuestionRepository { get; }
    protected FaqQuestionManager FaqQuestionManager { get; }

    public FaqQuestionAdminAppService(
        IFaqQuestionRepository faqQuestionRepository,
        FaqQuestionManager faqQuestionManager)
    {
        FaqQuestionRepository = faqQuestionRepository;
        FaqQuestionManager = faqQuestionManager;
    }

    public virtual async Task<FaqQuestionDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<FaqQuestion, FaqQuestionDto>(await FaqQuestionRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<FaqQuestionDto>> GetListAsync(FaqQuestionListFilterDto input)
    {
        var questions = await FaqQuestionRepository.GetListAsync(
            input.SectionId,
            input.Filter,
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount);

        return new PagedResultDto<FaqQuestionDto>
        {
            Items = new List<FaqQuestionDto>(
                ObjectMapper.Map<List<FaqQuestion>, List<FaqQuestionDto>>(questions)
            ),
            TotalCount = await FaqQuestionRepository.GetCountAsync(input.SectionId, input.Filter)
        };
    }

    [Authorize(CmsKitProAdminPermissions.Faqs.Create)]
    public virtual async Task<FaqQuestionDto> CreateAsync(CreateFaqQuestionDto input)
    {
        var question = await FaqQuestionManager.CreateAsync(
            input.SectionId,
            input.Title,
            input.Text,
            input.Order);

        var newQuestion = await FaqQuestionRepository.InsertAsync(question);

        return ObjectMapper.Map<FaqQuestion, FaqQuestionDto>(newQuestion);
    }

    [Authorize(CmsKitProAdminPermissions.Faqs.Update)]
    public virtual async Task<FaqQuestionDto> UpdateAsync(Guid id, UpdateFaqQuestionDto input)
    {
        var question = await FaqQuestionRepository.GetAsync(id);

        await FaqQuestionManager.UpdateTitle(question, input.Title);

        question.SetText(input.Text);
        question.Order = input.Order;
        
        var updatedSection = await FaqQuestionRepository.UpdateAsync(question);

        return ObjectMapper.Map<FaqQuestion, FaqQuestionDto>(updatedSection);
    }

    [Authorize(CmsKitProAdminPermissions.Faqs.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await FaqQuestionRepository.DeleteAsync(id);
    }
}