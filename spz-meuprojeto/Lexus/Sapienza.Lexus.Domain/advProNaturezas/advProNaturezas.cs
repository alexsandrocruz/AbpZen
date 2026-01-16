// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProNaturezas;

/// <summary>
/// advProNaturezas entity
/// </summary>
public class advProNaturezas : FullAuditedAggregateRoot<Guid>
{
    public int? idNatureza { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? mostraHistoricoNumeros { get; set; }
    public bool? recebeAcordo { get; set; }
    public bool? recebeRPV { get; set; }
    public bool? recebePrecatorio { get; set; }
    public bool? recebeAlvara { get; set; }
    public int? idArea { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advProfissionaisNaturezasId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas? advProNaturezasNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProNaturezas()
    {
        // Required by EF Core
    }

    public advProNaturezas(Guid id) : base(id)
    {
    }
}
