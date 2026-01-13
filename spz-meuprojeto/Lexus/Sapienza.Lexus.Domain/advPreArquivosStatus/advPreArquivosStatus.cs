// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreArquivosStatus;

/// <summary>
/// advPreArquivosStatus entity
/// </summary>
public class advPreArquivosStatus : FullAuditedAggregateRoot<Guid>
{
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreArquivosStatus()
    {
        // Required by EF Core
    }

    public advPreArquivosStatus(Guid id) : base(id)
    {
    }
}
