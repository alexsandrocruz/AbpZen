// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesConvertidos;

/// <summary>
/// advClientesConvertidos entity
/// </summary>
public class advClientesConvertidos : FullAuditedAggregateRoot<Guid>
{
    public int? idRegistro { get; set; }
    public int idCliente { get; set; }
    public string data { get; set; } = string.Empty;
    public DateTime? tsInclusao { get; set; }
    public string? convertidoPor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advClientesConvertidos()
    {
        // Required by EF Core
    }

    public advClientesConvertidos(Guid id) : base(id)
    {
    }
}
