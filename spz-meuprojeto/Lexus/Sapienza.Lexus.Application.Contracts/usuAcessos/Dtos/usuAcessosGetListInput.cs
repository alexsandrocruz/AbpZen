using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.usuAcessos.Dtos;

[Serializable]
public class usuAcessosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idAcesso { get; set; }
    public int? idUsuario { get; set; }
    public DateTime? data { get; set; }
    public string? ip { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
