using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Forms.Answers;

public class Answer : Entity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; set; }
    public virtual Guid QuestionId { get; private set; }
    public virtual Guid FormResponseId { get; private set; }
    public virtual Guid? ChoiceId { get; private set; }
    public virtual DateTime AnswerDate { get; private set; }
    [CanBeNull] public virtual string Value { get; private set; }

    protected Answer()
    {
    }

    public Answer(Guid id, Guid formResponseId, Guid questionId, Guid? choiceId, [CanBeNull] string value, Guid? tenantId = null) : base(id)
    {
        TenantId = tenantId;
        FormResponseId = formResponseId;
        QuestionId = questionId;
        Value = value;
        ChoiceId = choiceId;
        AnswerDate = DateTime.Now;
    }

    public virtual void UpdateAnswer(string newValue, Guid? choiceId)
    {
        ChoiceId = choiceId;
        Value = newValue;
        AnswerDate = DateTime.Now;
    }
}
