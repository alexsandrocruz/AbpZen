// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProfissionaisEstados;

/// <summary>
/// advProfissionaisEstados entity
/// </summary>
public class advProfissionaisEstados : FullAuditedAggregateRoot<Guid>
{
    public int? idProfissionalEstado { get; set; }
    public int idProfissional { get; set; }
    public string? estado { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advProfissionais.advProfissionais> advProfissionaisEstadoses { get; set; } = new List<Sapienza.Lexus.advProfissionais.advProfissionais>();

    protected advProfissionaisEstados()
    {
        // Required by EF Core
    }

    public advProfissionaisEstados(Guid id) : base(id)
    {
    }
}
