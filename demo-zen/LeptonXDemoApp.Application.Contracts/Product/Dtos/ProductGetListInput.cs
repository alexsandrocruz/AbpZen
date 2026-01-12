using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Product.Dtos;

[Serializable]
public class ProductGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public string? Price { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? CategoryId { get; set; }
}
