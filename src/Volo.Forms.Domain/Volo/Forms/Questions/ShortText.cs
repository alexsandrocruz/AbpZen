using System;

namespace Volo.Forms.Questions;

[QuestionType(QuestionTypes.ShortText)]
public class ShortText : QuestionBase, IRequired
{
    public bool IsRequired { get; set; }

    protected ShortText()
    {
    }

    public ShortText(Guid id, Guid? tenantId = null) : base(id, tenantId)
    {
    }
}
