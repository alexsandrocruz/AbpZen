// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProEscritorios;

/// <summary>
/// advProEscritorios entity
/// </summary>
public class advProEscritorios : FullAuditedAggregateRoot<Guid>
{
    public int? idEscritorio { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCentroCusto { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProEscritorios()
    {
        // Required by EF Core
    }

    public advProEscritorios(Guid id) : base(id)
    {
    }
}
