using System.Collections.Generic;
using Volo.Forms.Questions;

namespace Volo.Forms.Forms;

public class FormWithQuestions
{
    public Form Form { get; set; }

    public List<QuestionBase> Questions { get; set; }
}
