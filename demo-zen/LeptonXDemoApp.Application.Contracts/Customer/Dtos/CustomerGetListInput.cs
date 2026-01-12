using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Customer.Dtos;

[Serializable]
public class CustomerGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
