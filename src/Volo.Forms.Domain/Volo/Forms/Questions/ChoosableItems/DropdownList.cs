using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions.ChoosableItems;

[QuestionType(QuestionTypes.DropdownList)]
public class DropdownList : QuestionBase, IChoosable, IRequired
{
    public virtual bool IsRequired { get; set; } = false;
    public virtual Collection<Choice> Choices { private set; get; }

    protected DropdownList()
    {
    }

    public DropdownList(Guid id, Guid? tenantId = null) : base(id, tenantId)
    {
        Choices = new Collection<Choice>();
    }

    public ICollection<Choice> GetChoices()
    {
        return Choices.OrderBy(q => q.Index).ToList();
    }

    public void ClearChoices()
    {
        Choices.Clear();
    }

    public void AddChoice(Guid id, int index, string value, bool isCorrect = false, Guid? tenantId = null)
    {
        Choices.Add(
            new Choice(id: id, choosableQuestionId: Id, index: index, value: value, isCorrect: isCorrect, tenantId: tenantId)
        );
    }

    public void AddChoice(Guid id, string value, bool isCorrect = false, Guid? tenantId = null)
    {
        AddChoice(id: id, index: (Choices.Count - 1), value: value, isCorrect: isCorrect, tenantId: tenantId);
    }

    public void AddChoices(List<(Guid id, string value, bool isCorrect)> choices, Guid? tenantId = null)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            AddChoice(id: choices[i].id, index: i + 1, value: choices[i].value, isCorrect: choices[i].isCorrect, tenantId: tenantId);
        }
    }

    public void MoveChoice(Guid choiceId, int newIndex)
    {
        var choice = Choices.First(q => q.Id == choiceId);
        choice.UpdateIndex(newIndex);

        for (int i = newIndex; i < Choices.Count; i++)
        {
            Choices[i].UpdateIndex(i++);
        }
    }
}