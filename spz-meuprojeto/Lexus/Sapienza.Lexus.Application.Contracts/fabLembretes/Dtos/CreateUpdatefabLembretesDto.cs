using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabLembretes.Dtos;

[Serializable]
public class CreateUpdatefabLembretesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLembrete { get; set; }
    [Required]
    public Guid IdentityUserId { get; set; }
    public string mensagem { get; set; }
    public string destino { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string incluidoPor { get; set; }
    public bool? lido { get; set; }
    public string tipo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
