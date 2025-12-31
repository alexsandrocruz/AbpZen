using System;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Faqs;

public interface IFaqQuestionAdminAppService
      : ICrudAppService<
        FaqQuestionDto,
        FaqQuestionDto,
        Guid,
        FaqQuestionListFilterDto,
        CreateFaqQuestionDto,
        UpdateFaqQuestionDto>
{
}
