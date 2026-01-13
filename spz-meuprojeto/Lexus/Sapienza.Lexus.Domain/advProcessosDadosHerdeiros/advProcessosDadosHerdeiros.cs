// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

/// <summary>
/// advProcessosDadosHerdeiros entity
/// </summary>
public class advProcessosDadosHerdeiros : FullAuditedAggregateRoot<Guid>
{
    public int? idHerdeiro { get; set; }
    public int idProcesso { get; set; }
    public int? sequencia { get; set; }
    public int? bancarioBancoId { get; set; }
    public string? bancarioTipoConta { get; set; }
    public string? bancarioAgencia { get; set; }
    public string? bancarioConta { get; set; }
    public string? bancarioFavorecido { get; set; }
    public string? bancarioCpf { get; set; }
    public double? bancarioPerc { get; set; }
    public double? bancarioTarifa { get; set; }
    public string? bancarioTarifaParcelas { get; set; }
    public int? idHonorario { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProcessosDadosHerdeiros()
    {
        // Required by EF Core
    }

    public advProcessosDadosHerdeiros(Guid id) : base(id)
    {
    }
}
