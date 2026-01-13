using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finPrestacaoContas.Dtos;

[Serializable]
public class CreateUpdatefinPrestacaoContasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idPrestacao { get; set; }
    [Required]
    public int idLancamento { get; set; }
    public double? levantado { get; set; }
    public double? irpj { get; set; }
    public double? carta { get; set; }
    public double? honorarios { get; set; }
    public double? tarifa { get; set; }
    public double? liquidoRecebido { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
