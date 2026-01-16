using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.usuDistancias.Dtos;

[Serializable]
public class CreateUpdateusuDistanciasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idDistancia { get; set; }
    [Required]
    public Guid IdentityUserId { get; set; }
    public string estado { get; set; }
    public string cidade { get; set; }
    public int? km { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
