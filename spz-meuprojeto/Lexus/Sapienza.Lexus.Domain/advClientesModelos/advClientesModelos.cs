// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesModelos;

/// <summary>
/// advClientesModelos entity
/// </summary>
public class advClientesModelos : FullAuditedAggregateRoot<Guid>
{
    public int? idModelo { get; set; }
    public string? titulo { get; set; }
    public string? conteudo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advClientesModelos()
    {
        // Required by EF Core
    }

    public advClientesModelos(Guid id) : base(id)
    {
    }
}
