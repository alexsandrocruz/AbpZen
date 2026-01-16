using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finExtrato.Dtos;

[Serializable]
public class finExtratoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idExtrato { get; set; }
    public int? idConta { get; set; }
    public int? idLancamento { get; set; }
    public bool? transferencia { get; set; }
    public int? idExtratoRel { get; set; }
    public string? data { get; set; }
    public string? descricao { get; set; }
    public double? credito { get; set; }
    public double? debito { get; set; }
    public bool? conferido { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUsuarioInclusao { get; set; }
    public int? idUsuarioAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
