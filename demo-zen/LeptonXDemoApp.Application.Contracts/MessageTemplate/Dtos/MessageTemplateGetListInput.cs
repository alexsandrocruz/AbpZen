using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.MessageTemplate.Dtos;

[Serializable]
public class MessageTemplateGetListInput : PagedAndSortedResultRequestDto
{
    public string? Title { get; set; }
    public string? Body { get; set; }
    public MessageTypeEnum? MessageType { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
