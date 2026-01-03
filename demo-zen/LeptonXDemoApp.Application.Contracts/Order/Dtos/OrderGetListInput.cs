using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Order.Dtos;

[Serializable]
public class OrderGetListInput : PagedAndSortedResultRequestDto
{
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public Guid? CustomerId { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
