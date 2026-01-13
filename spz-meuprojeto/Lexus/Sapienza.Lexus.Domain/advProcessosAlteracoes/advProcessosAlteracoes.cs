// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProcessosAlteracoes;

/// <summary>
/// advProcessosAlteracoes entity
/// </summary>
public class advProcessosAlteracoes : FullAuditedAggregateRoot<Guid>
{
    public int? idProcessoAlteracao { get; set; }
    public int idProcesso { get; set; }
    public int idUsuario { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? texto { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProcessosAlteracoes()
    {
        // Required by EF Core
    }

    public advProcessosAlteracoes(Guid id) : base(id)
    {
    }
}
