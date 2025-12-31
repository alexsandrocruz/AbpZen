using System.Collections.Generic;
using Volo.Forms.Answers;
using Volo.Forms.Questions;

namespace Volo.Forms.Forms;

public class FormWithAnswers
{
    public Form Form { get; set; }

    public List<QuestionBase> Items { get; set; }

    public List<Answer> Answers { get; set; }
}
