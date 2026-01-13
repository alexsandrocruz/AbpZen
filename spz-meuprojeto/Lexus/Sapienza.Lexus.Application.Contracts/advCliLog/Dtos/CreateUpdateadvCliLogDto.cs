using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advCliLog.Dtos;

[Serializable]
public class CreateUpdateadvCliLogDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLog { get; set; }
    [Required]
    public int idCliente { get; set; }
    [Required]
    public int idUsuario { get; set; }
    [Required]
    public int acao { get; set; }
    [Required]
    public int idArea { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idResponsavel { get; set; }
    public DateTime? dataAgendamento { get; set; }
    public int? inssIdTipoBeneficio { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
