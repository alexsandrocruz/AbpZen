// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabConfig;

/// <summary>
/// fabConfig entity
/// </summary>
public class fabConfig : FullAuditedAggregateRoot<Guid>
{
    public int? idConfig { get; set; }
    public string? imagemLogin { get; set; }
    public string? imagemLoginCentral { get; set; }
    public string? imagemLoginTickets { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? precoCombustivel { get; set; }
    public string? dataBloqueioFinanceiro { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabConfig()
    {
        // Required by EF Core
    }

    public fabConfig(Guid id) : base(id)
    {
    }
}
