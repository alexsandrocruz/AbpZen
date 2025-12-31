using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqSectionDto : FullAuditedEntityDto<Guid>
{
    public string GroupName { get; set; }
    
    public string Name { get; set; }
    
    public int Order { get; set; }
}
