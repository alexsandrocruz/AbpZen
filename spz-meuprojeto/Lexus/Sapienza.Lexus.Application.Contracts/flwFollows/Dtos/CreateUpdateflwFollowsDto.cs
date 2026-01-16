using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.flwFollows.Dtos;

[Serializable]
public class CreateUpdateflwFollowsDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idFollow { get; set; }
    [Required]
    public int idCliente { get; set; }
    [Required]
    public int idAcao { get; set; }
    [Required]
    public int idTipo { get; set; }
    [Required]
    public Guid IdentityUserId { get; set; }
    [Required]
    public string data { get; set; } = string.Empty;
    public int? horario { get; set; }
    public string comentario { get; set; }
    public bool? finalizado { get; set; }
    public string dataFinalizacao { get; set; }
    public int? horarioFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idOportunidade { get; set; }
    public bool? chegou { get; set; }
    public DateTime? tsChegou { get; set; }
    public bool? naoComparecimento { get; set; }
    public bool? prioridade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
