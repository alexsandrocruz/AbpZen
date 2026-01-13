// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finContasClientes;

/// <summary>
/// finContasClientes entity
/// </summary>
public class finContasClientes : FullAuditedAggregateRoot<Guid>
{
    public int? idContaCliente { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? cor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finContasClientes()
    {
        // Required by EF Core
    }

    public finContasClientes(Guid id) : base(id)
    {
    }
}
