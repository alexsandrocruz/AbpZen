using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.Case.Dtos;

[Serializable]
public class CaseGetListInput : PagedAndSortedResultRequestDto
{
    public string? CaseNumber { get; set; }
    public string? Title { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
