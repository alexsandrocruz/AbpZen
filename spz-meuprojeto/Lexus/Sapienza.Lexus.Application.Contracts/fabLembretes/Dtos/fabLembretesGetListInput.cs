using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabLembretes.Dtos;

[Serializable]
public class fabLembretesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLembrete { get; set; }
    public Guid? IdentityUserId { get; set; }
    public string? mensagem { get; set; }
    public string? destino { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? incluidoPor { get; set; }
    public bool? lido { get; set; }
    public string? tipo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
