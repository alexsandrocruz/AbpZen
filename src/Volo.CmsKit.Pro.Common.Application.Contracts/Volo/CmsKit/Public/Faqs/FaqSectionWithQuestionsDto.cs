using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Faqs;

[Serializable]
public class FaqSectionWithQuestionsDto
{
    public FaqSectionDto Section { get; set; }

    public List<FaqQuestionDto> Questions { get; set; }

    public FaqSectionWithQuestionsDto()
    {
        Questions = new List<FaqQuestionDto>();
    }
}
