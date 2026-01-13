using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesINSS.Dtos;

[Serializable]
public class advClientesINSSGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idInssAgendado { get; set; }
    public int? idCliente { get; set; }
    public bool? inssAgendado { get; set; }
    public string? inssData { get; set; }
    public int? inssIdTipoBeneficio { get; set; }
    public int? inssIdPosto { get; set; }
    public string? inssResultado { get; set; }
    public string? tsInclusao { get; set; }
    public string? tsAlteracao { get; set; }
    public bool? inssResultadoIndicadorOculto { get; set; }
    public int? inssResponsavel { get; set; }
    public string? inssProtocolo { get; set; }
    public int? inssIdUsuarioInclusao { get; set; }
    public int? idStatus { get; set; }
    public string? dataFinalizacao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
