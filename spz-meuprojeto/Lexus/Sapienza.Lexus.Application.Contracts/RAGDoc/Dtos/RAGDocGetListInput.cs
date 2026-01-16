using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.RAGDoc.Dtos;

[Serializable]
public class RAGDocGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
