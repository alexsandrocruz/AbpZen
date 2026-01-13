using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advVerbas.Dtos;

[Serializable]
public class advVerbasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idVerba { get; set; }
    public int? idTipo { get; set; }
    public int? idProfissional { get; set; }
    public int? idProcesso { get; set; }
    public int? idLancamento { get; set; }
    public double? valor { get; set; }
    public string? dataDe { get; set; }
    public string? dataAte { get; set; }
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public bool? comprovante { get; set; }
    public string? comprovanteArquivo { get; set; }
    public bool? solicitacao { get; set; }
    public bool? aceito { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
