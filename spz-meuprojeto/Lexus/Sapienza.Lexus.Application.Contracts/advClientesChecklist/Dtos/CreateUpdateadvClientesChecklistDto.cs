using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesChecklist.Dtos;

[Serializable]
public class CreateUpdateadvClientesChecklistDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idClienteChecklist { get; set; }
    [Required]
    public int idCliente { get; set; }
    [Required]
    public int idTipoArquivo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
