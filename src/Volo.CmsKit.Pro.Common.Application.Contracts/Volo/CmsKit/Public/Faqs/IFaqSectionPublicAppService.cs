using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Faqs;

public interface IFaqSectionPublicAppService : IApplicationService
{
    Task<List<FaqSectionWithQuestionsDto>> GetListSectionWithQuestionsAsync(FaqSectionListFilterInput input);
}