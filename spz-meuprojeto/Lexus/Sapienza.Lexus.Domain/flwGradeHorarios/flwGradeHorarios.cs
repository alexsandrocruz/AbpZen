// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.flwGradeHorarios;

/// <summary>
/// flwGradeHorarios entity
/// </summary>
public class flwGradeHorarios : FullAuditedAggregateRoot<Guid>
{
    public int? idGrade { get; set; }
    public int idHistoricoTipo { get; set; }
    public int? manhaHorarioInicial { get; set; }
    public int? manhaIntervalo { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeHorarioInicial { get; set; }
    public int? tardeIntervalo { get; set; }
    public int? tardeQtde { get; set; }
    public bool? dom { get; set; }
    public bool? seg { get; set; }
    public bool? ter { get; set; }
    public bool? qua { get; set; }
    public bool? qui { get; set; }
    public bool? sex { get; set; }
    public bool? sab { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected flwGradeHorarios()
    {
        // Required by EF Core
    }

    public flwGradeHorarios(Guid id) : base(id)
    {
    }
}
