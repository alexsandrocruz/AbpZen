// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProfissionais;

/// <summary>
/// advProfissionais entity
/// </summary>
public class advProfissionais : FullAuditedAggregateRoot<Guid>
{
    public int? idProfissional { get; set; }
    public int idUsuario { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProfissionais()
    {
        // Required by EF Core
    }

    public advProfissionais(Guid id) : base(id)
    {
    }
}
