using System;
using System.Collections.Generic;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions.ChoosableItems;

public interface IChoosable
{
    public void AddChoice(Guid id, int index, string value, bool isCorrect = false, Guid? tenantId = null);
    public void AddChoices(List<(Guid id, string value, bool isCorrect)> choices, Guid? tenantId = null);
    public void MoveChoice(Guid id, int newIndex);
    public ICollection<Choice> GetChoices();
    public void ClearChoices();
    // public void SetChoiceValues(List<string> values);
}
