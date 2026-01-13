// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabPermissoes;

/// <summary>
/// fabPermissoes entity
/// </summary>
public class fabPermissoes : FullAuditedAggregateRoot<Guid>
{
    public int? idPermissao { get; set; }
    public int idPermissaoTipo { get; set; }
    public bool? modulo { get; set; }
    public string? descricao { get; set; }
    public string? varSession { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabPermissoes()
    {
        // Required by EF Core
    }

    public fabPermissoes(Guid id) : base(id)
    {
    }
}
