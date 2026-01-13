using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advPreLogStatus.Dtos;

[Serializable]
public class CreateUpdateadvPreLogStatusDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLog { get; set; }
    [Required]
    public int idProcesso { get; set; }
    [Required]
    public int idStatus { get; set; }
    public DateTime? tsInclusao { get; set; }
    public bool? conversao { get; set; }
    public DateTime? tsConversao { get; set; }
    public bool? perdido { get; set; }
    public DateTime? tsPerdido { get; set; }
    public int? diasCorridosDoAnterior { get; set; }
    public string usuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
