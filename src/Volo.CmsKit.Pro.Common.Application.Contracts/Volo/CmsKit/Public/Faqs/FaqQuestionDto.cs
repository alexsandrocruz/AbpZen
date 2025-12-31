using System;

namespace Volo.CmsKit.Public.Faqs;

[Serializable]
public class FaqQuestionDto
{
    public string Title { get; set; }

    public string Text { get; set; }
}
