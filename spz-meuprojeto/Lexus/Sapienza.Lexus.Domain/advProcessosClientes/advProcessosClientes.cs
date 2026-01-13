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

    protected advProcessosClientes()
    {
        // Required by EF Core
    }

    public advProcessosClientes(Guid id) : base(id)
    {
    }
}
