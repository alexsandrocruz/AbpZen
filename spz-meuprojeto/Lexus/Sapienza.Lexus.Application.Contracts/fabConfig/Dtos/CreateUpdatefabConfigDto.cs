using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabConfig.Dtos;

[Serializable]
public class CreateUpdatefabConfigDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idConfig { get; set; }
    public string imagemLogin { get; set; }
    public string imagemLoginCentral { get; set; }
    public string imagemLoginTickets { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? precoCombustivel { get; set; }
    public string dataBloqueioFinanceiro { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
