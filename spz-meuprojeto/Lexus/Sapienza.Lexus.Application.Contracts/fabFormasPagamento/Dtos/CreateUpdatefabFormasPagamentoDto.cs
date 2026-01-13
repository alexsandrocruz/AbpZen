using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabFormasPagamento.Dtos;

[Serializable]
public class CreateUpdatefabFormasPagamentoDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idFormaPagamento { get; set; }
    public string titulo { get; set; }
    public int? ordem { get; set; }
    public bool? padrao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public bool? contasPagar { get; set; }
    public bool? compras { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
