// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPostosINSS;

/// <summary>
/// advPostosINSS entity
/// </summary>
public class advPostosINSS : FullAuditedAggregateRoot<Guid>
{
    public int? idPosto { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid advClientesId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advClientes.advClientes advPostosINSSNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPostosINSS()
    {
        // Required by EF Core
    }

    public advPostosINSS(Guid id) : base(id)
    {
    }
}
