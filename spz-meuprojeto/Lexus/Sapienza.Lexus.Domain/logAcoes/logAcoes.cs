// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.logAcoes;

/// <summary>
/// logAcoes entity
/// </summary>
public class logAcoes : FullAuditedAggregateRoot<Guid>
{
    public int? idLog { get; set; }
    public string? area { get; set; }
    public string? acao { get; set; }
    public string? usuario { get; set; }
    public string? motivo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public int? idCompromisso { get; set; }
    public int? idTarefa { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected logAcoes()
    {
        // Required by EF Core
    }

    public logAcoes(Guid id) : base(id)
    {
    }
}
