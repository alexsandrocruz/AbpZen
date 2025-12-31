using System.Collections.Generic;
using Volo.Forms.Answers;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions;

public class QuestionWithAnswers
{
    public QuestionBase Question { get; set; }
    public List<Answer> Answers { get; set; }
    public List<Choice> Choices { get; set; }

    public QuestionWithAnswers()
    {
    }
}
