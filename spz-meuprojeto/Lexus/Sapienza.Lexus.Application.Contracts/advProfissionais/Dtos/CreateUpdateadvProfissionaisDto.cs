using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProfissionais.Dtos;

[Serializable]
public class CreateUpdateadvProfissionaisDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProfissional { get; set; }
    [Required]
    public Guid IdentityUserId { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advProfissionaisEstadosId { get; set; }
    public Guid? advProfissionaisNaturezasId { get; set; }
    public Guid? advVerbasId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
