using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ClientContact.Dtos;

[Serializable]
public class ClientContactGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
