using System.Collections.Generic;
using Volo.CmsKit.Public.Faqs;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Faqs;

public class FaqViewModel
{
    public bool ShowSectionName { get; set; }

    public List<FaqSectionWithQuestionsDto> SectionWithQuestions { get; set; }

    public FaqViewModel()
    {
        SectionWithQuestions = new List<FaqSectionWithQuestionsDto>();
    }
}