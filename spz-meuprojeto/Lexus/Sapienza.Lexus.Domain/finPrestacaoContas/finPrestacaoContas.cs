// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finPrestacaoContas;

/// <summary>
/// finPrestacaoContas entity
/// </summary>
public class finPrestacaoContas : FullAuditedAggregateRoot<Guid>
{
    public int? idPrestacao { get; set; }
    public int idLancamento { get; set; }
    public double? levantado { get; set; }
    public double? irpj { get; set; }
    public double? carta { get; set; }
    public double? honorarios { get; set; }
    public double? tarifa { get; set; }
    public double? liquidoRecebido { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.finLancamentos.finLancamentos> finPrestacaoContases { get; set; } = new List<Sapienza.Lexus.finLancamentos.finLancamentos>();

    protected finPrestacaoContas()
    {
        // Required by EF Core
    }

    public finPrestacaoContas(Guid id) : base(id)
    {
    }
}
