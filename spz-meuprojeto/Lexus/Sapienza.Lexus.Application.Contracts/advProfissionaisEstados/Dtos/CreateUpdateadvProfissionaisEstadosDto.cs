using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProfissionaisEstados.Dtos;

[Serializable]
public class CreateUpdateadvProfissionaisEstadosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProfissionalEstado { get; set; }
    [Required]
    public int idProfissional { get; set; }
    public string estado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
