using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finProcuracoesRPV.Dtos;

[Serializable]
public class CreateUpdatefinProcuracoesRPVDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProcuracao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public bool? impressa { get; set; }
    public DateTime? tsImpressa { get; set; }
    public bool? assinada { get; set; }
    public DateTime? tsAssinatura { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
