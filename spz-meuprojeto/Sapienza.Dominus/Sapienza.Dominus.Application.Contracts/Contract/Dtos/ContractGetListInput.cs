using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Contract.Dtos;

[Serializable]
public class ContractGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
