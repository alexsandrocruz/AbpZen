using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProcessosClientes.Dtos;

[Serializable]
public class CreateUpdateadvProcessosClientesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProcessoCliente { get; set; }
    [Required]
    public int idProcesso { get; set; }
    [Required]
    public int idCliente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
