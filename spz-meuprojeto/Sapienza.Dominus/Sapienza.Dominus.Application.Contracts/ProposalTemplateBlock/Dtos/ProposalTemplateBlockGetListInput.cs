using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalTemplateBlock.Dtos;

[Serializable]
public class ProposalTemplateBlockGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
