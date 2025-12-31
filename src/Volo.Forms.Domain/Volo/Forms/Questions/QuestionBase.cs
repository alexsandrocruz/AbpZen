using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Forms.Questions.ChoosableItems;

namespace Volo.Forms.Questions;

public abstract class QuestionBase : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid FormId { get; protected set; }
    public virtual int Index { get; private set; }
    public virtual string Title { get; private set; }
    public virtual string Description { get; private set; }

    protected QuestionBase()
    {
    }

    internal QuestionBase(Guid id, Guid? tenantId = null) : base(id)
    {
        TenantId = tenantId;
    }

    public virtual Guid GetId()
    {
        return Id;
    }

    public virtual int GetIndex()
    {
        return Index;
    }

    public virtual QuestionBase SetIndex(int index)
    {
        Index = index;
        return this;
    }

    public virtual string GetTitle()
    {
        return Title;
    }

    public virtual QuestionBase SetTitle(string title)
    {
        Title = Check.NotNullOrEmpty(title, nameof(title), FormConsts.MaxTitleLength);
        return this;
    }

    public virtual string GetDescription()
    {
        return Description;
    }

    public virtual QuestionBase SetDescription(string description)
    {
        Description = Check.Length(description, nameof(description), FormConsts.MaxDescriptionLength);
        return this;
    }

    public virtual QuestionTypes GetQuestionType()
    {
        var questionTypeAttribute = (QuestionTypeAttribute)Attribute.GetCustomAttribute(GetType(), typeof(QuestionTypeAttribute));

        if (questionTypeAttribute == null)
        {
            throw new AbpException($"QuestionTypeAttribute is not set for {GetType()}");
        }

        return questionTypeAttribute.QuestionType;
    }

    public virtual QuestionBase SetFormId(Guid formId)
    {
        FormId = formId;
        return this;
    }

    public virtual void SetRequired(bool isRequired)
    {
        if (this is IRequired)
        {
            ((IRequired)this).IsRequired = isRequired;
        }
    }

    public virtual void SetOtherOption(bool hasOtherOption)
    {
        if (this is IHasOtherOption)
        {
            ((IHasOtherOption)this).HasOtherOption = hasOtherOption;
        }
    }
}