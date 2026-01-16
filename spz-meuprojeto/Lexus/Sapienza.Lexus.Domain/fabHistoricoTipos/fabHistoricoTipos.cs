// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabHistoricoTipos;

/// <summary>
/// fabHistoricoTipos entity
/// </summary>
public class fabHistoricoTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idHistoricoTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? tipoMarcacoes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? flwConfigExcecoesId { get; set; }
    public Guid? flwGradeHorariosId { get; set; }
    public Guid? flwFollowsId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes? fabHistoricoTiposNav { get; set; }
    public virtual Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios? fabHistoricoTiposNav1 { get; set; }
    public virtual Sapienza.Lexus.flwFollows.flwFollows? fabHistoricoTiposNav2 { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabHistoricoTipos()
    {
        // Required by EF Core
    }

    public fabHistoricoTipos(Guid id) : base(id)
    {
    }
}
