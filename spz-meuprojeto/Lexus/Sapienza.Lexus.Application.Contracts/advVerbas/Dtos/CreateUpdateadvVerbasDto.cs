using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advVerbas.Dtos;

[Serializable]
public class CreateUpdateadvVerbasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idVerba { get; set; }
    [Required]
    public int idTipo { get; set; }
    [Required]
    public int idProfissional { get; set; }
    public int? idProcesso { get; set; }
    public int? idLancamento { get; set; }
    public double? valor { get; set; }
    [Required]
    public string dataDe { get; set; } = string.Empty;
    [Required]
    public string dataAte { get; set; } = string.Empty;
    public string estado { get; set; }
    public string cidade { get; set; }
    public bool? comprovante { get; set; }
    public string comprovanteArquivo { get; set; }
    public bool? solicitacao { get; set; }
    public bool? aceito { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string incluidoPor { get; set; }
    public string alteradoPor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
