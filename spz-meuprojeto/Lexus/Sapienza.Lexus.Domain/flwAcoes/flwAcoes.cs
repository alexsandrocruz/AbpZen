// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.flwAcoes;

/// <summary>
/// flwAcoes entity
/// </summary>
public class flwAcoes : FullAuditedAggregateRoot<Guid>
{
    public int? idAcao { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? diasReagendamento { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? flwFollowsId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.flwFollows.flwFollows? flwAcoesNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected flwAcoes()
    {
        // Required by EF Core
    }

    public flwAcoes(Guid id) : base(id)
    {
    }
}
