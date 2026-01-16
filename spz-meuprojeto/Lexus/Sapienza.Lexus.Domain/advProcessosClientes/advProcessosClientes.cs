// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProcessosClientes;

/// <summary>
/// advProcessosClientes entity
/// </summary>
public class advProcessosClientes : FullAuditedAggregateRoot<Guid>
{
    public int? idProcessoCliente { get; set; }
    public int idProcesso { get; set; }
    public int idCliente { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> advProcessosClienteses { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();
    public virtual ICollection<Sapienza.Lexus.advProcessos.advProcessos> advProcessosClientesesCollection { get; set; } = new List<Sapienza.Lexus.advProcessos.advProcessos>();

    protected advProcessosClientes()
    {
        // Required by EF Core
    }

    public advProcessosClientes(Guid id) : base(id)
    {
    }
}
