// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finProcuracoesRPV;

/// <summary>
/// finProcuracoesRPV entity
/// </summary>
public class finProcuracoesRPV : FullAuditedAggregateRoot<Guid>
{
    public int? idProcuracao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public bool? impressa { get; set; }
    public DateTime? tsImpressa { get; set; }
    public bool? assinada { get; set; }
    public DateTime? tsAssinatura { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finProcuracoesRPV()
    {
        // Required by EF Core
    }

    public finProcuracoesRPV(Guid id) : base(id)
    {
    }
}
