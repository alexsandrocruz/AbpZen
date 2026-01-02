using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Product.Dtos;

[Serializable]
public class ProductGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public Guid? CategoryId { get; set; }
}
