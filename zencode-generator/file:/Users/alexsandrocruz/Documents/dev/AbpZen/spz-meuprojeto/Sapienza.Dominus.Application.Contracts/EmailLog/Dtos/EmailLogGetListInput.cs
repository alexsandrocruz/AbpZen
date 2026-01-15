using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.EmailLog.Dtos;

[Serializable]
public class EmailLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
