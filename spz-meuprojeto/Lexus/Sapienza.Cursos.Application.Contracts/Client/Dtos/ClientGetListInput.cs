using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Cursos.Client.Dtos;

[Serializable]
public class ClientGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CpfCnpj { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
