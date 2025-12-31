using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Public.Faqs;

public class FaqSectionPublicAppService: PublicAppService, IFaqSectionPublicAppService
{
    protected IFaqSectionRepository FaqSectionRepository { get; }
    
    public FaqSectionPublicAppService(IFaqSectionRepository faqSectionRepository)
    {
        FaqSectionRepository = faqSectionRepository;
    }

    public async Task<List<FaqSectionWithQuestionsDto>> GetListSectionWithQuestionsAsync(FaqSectionListFilterInput input)
    {
        var sectionWithQuestions = await FaqSectionRepository.GetListSectionWithQuestionAsync(input.GroupName, input.SectionName);

        return ObjectMapper.Map<List<FaqSectionWithQuestions>, List<FaqSectionWithQuestionsDto>>(sectionWithQuestions);
    }
}
