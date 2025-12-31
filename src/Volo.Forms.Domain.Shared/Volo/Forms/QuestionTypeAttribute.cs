using System;

namespace Volo.Forms;

public class QuestionTypeAttribute : Attribute
{
    public QuestionTypes QuestionType { get; private set; }

    public QuestionTypeAttribute(QuestionTypes questionType)
    {
        QuestionType = questionType;
    }
}
