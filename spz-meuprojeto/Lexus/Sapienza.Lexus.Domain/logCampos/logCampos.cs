// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.logCampos;

/// <summary>
/// logCampos entity
/// </summary>
public class logCampos : FullAuditedAggregateRoot<Guid>
{
    public int? idLogCampo { get; set; }
    public int idLog { get; set; }
    public string? campo { get; set; }
    public string? dadoAnterior { get; set; }
    public string? dadoNovo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.logAcoes.logAcoes> logCamposes { get; set; } = new List<Sapienza.Lexus.logAcoes.logAcoes>();

    protected logCampos()
    {
        // Required by EF Core
    }

    public logCampos(Guid id) : base(id)
    {
    }
}
