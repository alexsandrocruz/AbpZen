using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.File.Dtos;

[Serializable]
public class FileGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
