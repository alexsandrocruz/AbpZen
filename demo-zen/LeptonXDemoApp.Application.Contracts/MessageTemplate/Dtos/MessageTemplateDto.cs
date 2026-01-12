using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.MessageTemplate.Dtos;

[Serializable]
public class MessageTemplateDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public MessageTypeEnum? MessageType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
