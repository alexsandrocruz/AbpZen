using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.LeadMessage.Dtos;

[Serializable]
public class LeadMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public string? Body { get; set; }
    public bool? Success { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? MessageTemplateId { get; set; }
    public Guid? LeadId { get; set; }
}
