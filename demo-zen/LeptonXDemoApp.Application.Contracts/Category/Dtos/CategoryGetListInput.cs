using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Category.Dtos;

[Serializable]
public class CategoryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
