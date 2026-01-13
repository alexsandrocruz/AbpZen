using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProcessosAlteracoes.Dtos;

[Serializable]
public class CreateUpdateadvProcessosAlteracoesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idProcessoAlteracao { get; set; }
    [Required]
    public int idProcesso { get; set; }
    [Required]
    public int idUsuario { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string texto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
