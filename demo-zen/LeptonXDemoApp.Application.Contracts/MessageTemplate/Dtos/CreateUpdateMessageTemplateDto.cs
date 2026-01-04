using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.MessageTemplate.Dtos;

[Serializable]
public class CreateUpdateMessageTemplateDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public MessageTypeEnum? MessageType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
