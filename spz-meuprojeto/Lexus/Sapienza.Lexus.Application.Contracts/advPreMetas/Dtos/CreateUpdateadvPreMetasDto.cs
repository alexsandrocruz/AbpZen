using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advPreMetas.Dtos;

[Serializable]
public class CreateUpdateadvPreMetasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idMeta { get; set; }
    public string tipo { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public int? idEscritorio { get; set; }
    public int? qtde { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
