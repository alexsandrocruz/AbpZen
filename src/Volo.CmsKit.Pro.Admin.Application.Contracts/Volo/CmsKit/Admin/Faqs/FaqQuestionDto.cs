using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqQuestionDto : FullAuditedEntityDto<Guid>
{
    public Guid SectionId { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public int Order { get; set; }
}
