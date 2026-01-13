using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesConvertidos.Dtos;

[Serializable]
public class CreateUpdateadvClientesConvertidosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idRegistro { get; set; }
    [Required]
    public int idCliente { get; set; }
    [Required]
    public string data { get; set; } = string.Empty;
    public DateTime? tsInclusao { get; set; }
    public string convertidoPor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
