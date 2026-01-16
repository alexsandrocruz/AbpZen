using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finRateiosPadrao.Dtos;

[Serializable]
public class CreateUpdatefinRateiosPadraoDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idPadrao { get; set; }
    [Required]
    public int idUnidade { get; set; }
    [Required]
    public int idCentroResultado { get; set; }
    [Required]
    public double porcentagem { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
