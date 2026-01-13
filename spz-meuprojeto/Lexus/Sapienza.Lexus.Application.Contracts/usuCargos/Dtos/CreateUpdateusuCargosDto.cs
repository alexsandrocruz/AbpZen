using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.usuCargos.Dtos;

[Serializable]
public class CreateUpdateusuCargosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idCargo { get; set; }
    [Required]
    public int idArea { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
