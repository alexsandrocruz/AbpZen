using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Aluno.Dtos;

[Serializable]
public class CreateUpdateAlunoDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }
    [Required]
    [StringLength(200)]
    public string Email { get; set; }
    [Required]
    [StringLength(20)]
    public string Matricula { get; set; }
    public DateTime? DataNascimento { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateAlunoTurmaDto>? AlunoTurmas { get; set; }
}
