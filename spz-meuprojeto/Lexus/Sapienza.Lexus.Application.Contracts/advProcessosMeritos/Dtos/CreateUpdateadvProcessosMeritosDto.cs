using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProcessosMeritos.Dtos;

[Serializable]
public class CreateUpdateadvProcessosMeritosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProcessoMerito { get; set; }
    [Required]
    public int idProcesso { get; set; }
    [Required]
    public int idMerito { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
