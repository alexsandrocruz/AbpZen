// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabFormasPagamento;

/// <summary>
/// fabFormasPagamento entity
/// </summary>
public class fabFormasPagamento : FullAuditedAggregateRoot<Guid>
{
    public int? idFormaPagamento { get; set; }
    public string? titulo { get; set; }
    public int? ordem { get; set; }
    public bool? padrao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public bool? contasPagar { get; set; }
    public bool? compras { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabFormasPagamento()
    {
        // Required by EF Core
    }

    public fabFormasPagamento(Guid id) : base(id)
    {
    }
}
