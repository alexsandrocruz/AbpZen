using System.Collections.Generic;

namespace Volo.CmsKit.Faqs;

public class FaqSectionWithQuestions
{
    public FaqSection Section { get; set; }

    public List<FaqQuestion> Questions { get; set; }
}
