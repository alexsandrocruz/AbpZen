using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProfissionaisNaturezas.Dtos;

[Serializable]
public class CreateUpdateadvProfissionaisNaturezasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProfissionalNatureza { get; set; }
    [Required]
    public int idProfissional { get; set; }
    [Required]
    public int idNatureza { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
