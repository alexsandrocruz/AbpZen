using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Faqs;

public interface IFaqSectionAdminAppService 
    : ICrudAppService<
        FaqSectionDto,
        FaqSectionWithQuestionCountDto,
        Guid,
        FaqSectionListFilterDto,
        CreateFaqSectionDto,
        UpdateFaqSectionDto>
{
    Task<Dictionary<string, FaqGroupInfoDto>> GetGroupsAsync();
}
