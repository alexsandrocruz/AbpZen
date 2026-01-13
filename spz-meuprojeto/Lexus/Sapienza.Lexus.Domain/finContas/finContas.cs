// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finContas;

/// <summary>
/// finContas entity
/// </summary>
public class finContas : FullAuditedAggregateRoot<Guid>
{
    public int? idConta { get; set; }
    public string? titulo { get; set; }
    public string? banco { get; set; }
    public string? agencia { get; set; }
    public string? conta { get; set; }
    public string? favorecido { get; set; }
    public double? limite { get; set; }
    public bool? padraoFluxo { get; set; }
    public bool? considerarIndicador { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? saldoInicial { get; set; }
    public bool? padrao { get; set; }
    public string? codigo { get; set; }
    public string? cor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finContas()
    {
        // Required by EF Core
    }

    public finContas(Guid id) : base(id)
    {
    }
}
