// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliTiposArquivos;

/// <summary>
/// advCliTiposArquivos entity
/// </summary>
public class advCliTiposArquivos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipoArquivo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? pasta { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliTiposArquivos()
    {
        // Required by EF Core
    }

    public advCliTiposArquivos(Guid id) : base(id)
    {
    }
}
