using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Faqs;

public class FaqQuestion : FullAuditedAggregateRoot<Guid>
{
    public virtual Guid SectionId { get; internal set; }
    
    public virtual string Title { get; protected set; }
    
    public virtual string Text { get; protected set; }
    
    public virtual int Order { get; set; }

    protected FaqQuestion()
    {

    }
    
    internal FaqQuestion(
        Guid id,
        Guid sectionId,
        [NotNull] string title,
        [NotNull] string text,
        int order = 0) : base(id)
    {
        SectionId = sectionId;
        SetTitle(title);
        SetText(text);
        Order = order;
    }

    internal virtual FaqQuestion SetTitle([NotNull] string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), FaqQuestionConst.MaxTitleLength);
        return this;
    }
    
    public virtual FaqQuestion SetText([NotNull] string text)
    {
        Text = Check.NotNullOrWhiteSpace(text, nameof(text), FaqQuestionConst.MaxTextLength);
        return this;
    }   
}