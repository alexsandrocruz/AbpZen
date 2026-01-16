// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finCentrosCusto;

/// <summary>
/// finCentrosCusto entity
/// </summary>
public class finCentrosCusto : FullAuditedAggregateRoot<Guid>
{
    public int? idCentroCusto { get; set; }
    public int? idUnidade { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padrao { get; set; }
    public double? porcentagemRateio { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? finLancamentosId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.finLancamentos.finLancamentos? finCentrosCustoNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finCentrosCusto()
    {
        // Required by EF Core
    }

    public finCentrosCusto(Guid id) : base(id)
    {
    }
}
