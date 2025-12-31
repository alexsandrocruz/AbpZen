using System;

namespace Volo.CmsKit.Faqs;

public class FaqSectionWithQuestionCount
{
    public virtual Guid Id { get; set; }

    public virtual string GroupName { get; set; }
    
    public virtual string Name { get; set; }
    
    public virtual int Order { get; set; }
    
    public int QuestionCount { get; set; }

    public DateTime CreationTime { get; set; } 
}