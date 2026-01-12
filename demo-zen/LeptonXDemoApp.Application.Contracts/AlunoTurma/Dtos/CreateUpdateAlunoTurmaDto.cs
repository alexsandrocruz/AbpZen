using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.AlunoTurma.Dtos;

[Serializable]
public class CreateUpdateAlunoTurmaDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public DateTime DataMatricula { get; set; }
    [StringLength(50)]
    public string Situacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    public Guid AlunoId { get; set; }
    [Required]
    public Guid TurmaId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
