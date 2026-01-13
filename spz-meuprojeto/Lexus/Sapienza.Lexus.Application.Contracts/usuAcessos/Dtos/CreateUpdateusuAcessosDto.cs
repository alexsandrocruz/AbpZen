using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.usuAcessos.Dtos;

[Serializable]
public class CreateUpdateusuAcessosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idAcesso { get; set; }
    [Required]
    public int idUsuario { get; set; }
    public DateTime? data { get; set; }
    public string ip { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
