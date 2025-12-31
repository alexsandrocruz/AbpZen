using System.Collections.Generic;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions;

public class QuestionWithChoices
{
    public QuestionBase Question { get; set; }
    public List<Choice> Choices { get; set; }

    public QuestionWithChoices()
    {
    }
}
