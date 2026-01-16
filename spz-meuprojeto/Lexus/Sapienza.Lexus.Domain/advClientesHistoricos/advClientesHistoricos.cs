// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesHistoricos;

/// <summary>
/// advClientesHistoricos entity
/// </summary>
public class advClientesHistoricos : FullAuditedAggregateRoot<Guid>
{
    public int? idHistorico { get; set; }
    public int idCliente { get; set; }
    public int? idProcesso { get; set; }
    public Guid IdentityUserId { get; set; }
    public int idTipoHistorico { get; set; }
    public string data { get; set; } = string.Empty;
    public string? hora { get; set; }
    public string? ocorrencia { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idOportunidade { get; set; }
    public string? depto { get; set; }
    public bool? prioritario { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos> advClientesHistoricoses { get; set; } = new List<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos>();
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> advClientesHistoricosesCollection { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();

    protected advClientesHistoricos()
    {
        // Required by EF Core
    }

    public advClientesHistoricos(Guid id) : base(id)
    {
    }
}
