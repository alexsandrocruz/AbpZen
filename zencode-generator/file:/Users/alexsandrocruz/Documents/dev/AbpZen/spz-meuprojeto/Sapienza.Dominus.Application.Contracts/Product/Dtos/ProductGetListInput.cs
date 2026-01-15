using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Product.Dtos;

[Serializable]
public class ProductGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
