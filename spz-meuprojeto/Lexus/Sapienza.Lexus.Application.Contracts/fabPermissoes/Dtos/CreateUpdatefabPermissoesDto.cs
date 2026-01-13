using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabPermissoes.Dtos;

[Serializable]
public class CreateUpdatefabPermissoesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idPermissao { get; set; }
    [Required]
    public int idPermissaoTipo { get; set; }
    public bool? modulo { get; set; }
    public string descricao { get; set; }
    public string varSession { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
