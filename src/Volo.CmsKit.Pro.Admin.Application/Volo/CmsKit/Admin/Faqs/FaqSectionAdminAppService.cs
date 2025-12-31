using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
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
public class FaqSectionAdminAppService : CmsKitProAdminAppService, IFaqSectionAdminAppService
{
    protected IFaqSectionRepository FaqSectionRepository { get; }
    protected FaqSectionManager FaqSectionManager { get; }
    protected FaqOptions FaqOptions { get; }

    public FaqSectionAdminAppService(
        IFaqSectionRepository faqSectionRepository,
        FaqSectionManager faqSectionManager,
        IOptions<FaqOptions> options)
    {
        FaqSectionRepository = faqSectionRepository;
        FaqSectionManager = faqSectionManager;
        FaqOptions = options.Value;
    }

    public virtual async Task<FaqSectionDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<FaqSection, FaqSectionDto>(await FaqSectionRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<FaqSectionWithQuestionCountDto>> GetListAsync(FaqSectionListFilterDto filterDto)
    {
        var sections = await FaqSectionRepository.GetListAsync(filterDto.Filter, filterDto.Sorting, filterDto.SkipCount, filterDto.MaxResultCount);

        return new PagedResultDto<FaqSectionWithQuestionCountDto>
        {
            Items = new List<FaqSectionWithQuestionCountDto>(
                ObjectMapper.Map<List<FaqSectionWithQuestionCount>, List<FaqSectionWithQuestionCountDto>>(sections)
            ),
            TotalCount = await FaqSectionRepository.GetCountAsync(filterDto.Filter)
        };
    }

    [Authorize(CmsKitProAdminPermissions.Faqs.Create)]
    public virtual async Task<FaqSectionDto> CreateAsync(CreateFaqSectionDto input)
    {
        var section = await FaqSectionManager.CreateAsync(input.GroupName,input.Name);
        
        section.Order = input.Order;

        await FaqSectionRepository.InsertAsync(section);

        return ObjectMapper.Map<FaqSection, FaqSectionDto>(section);
    }

    [Authorize(CmsKitProAdminPermissions.Faqs.Update)]
    public virtual async Task<FaqSectionDto> UpdateAsync(Guid id, UpdateFaqSectionDto input)
    {
        var section = await FaqSectionRepository.GetAsync(id);
        
        await FaqSectionManager.UpdateAsync(section, input.GroupName, input.Name);

        section.Order = input.Order;

        await FaqSectionRepository.UpdateAsync(section);

        return ObjectMapper.Map<FaqSection,FaqSectionDto>(section);
    }
    
    [Authorize(CmsKitProAdminPermissions.Faqs.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await FaqSectionRepository.DeleteAsync(id);
    }

    public virtual async Task<Dictionary<string, FaqGroupInfoDto>> GetGroupsAsync()
    {
        return await Task.FromResult(ObjectMapper.Map<Dictionary<string, FaqGroupInfo>, Dictionary<string, FaqGroupInfoDto>>(FaqOptions.Groups.ToDictionary()));
    }
}