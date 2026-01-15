using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.BlogCategory.Dtos;

[Serializable]
public class BlogCategoryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
