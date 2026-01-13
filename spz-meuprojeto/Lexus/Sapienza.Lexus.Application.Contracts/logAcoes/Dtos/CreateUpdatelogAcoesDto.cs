using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.logAcoes.Dtos;

[Serializable]
public class CreateUpdatelogAcoesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLog { get; set; }
    public string area { get; set; }
    public string acao { get; set; }
    public string usuario { get; set; }
    public string motivo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public int? idCompromisso { get; set; }
    public int? idTarefa { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
