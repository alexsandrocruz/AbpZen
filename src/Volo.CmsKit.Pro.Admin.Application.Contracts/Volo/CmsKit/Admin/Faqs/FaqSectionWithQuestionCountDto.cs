using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqSectionWithQuestionCountDto
{
    public virtual Guid Id { get; set; }

    public virtual string GroupName { get; set; }

    public virtual string Name { get; set; }

    public virtual int Order { get; set; }

    public int QuestionCount { get; set; }

    public virtual DateTime CreationTime { get; set; }
}