// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.flwConfigExcecoes;

/// <summary>
/// flwConfigExcecoes entity
/// </summary>
public class flwConfigExcecoes : FullAuditedAggregateRoot<Guid>
{
    public int? idConfig { get; set; }
    public string? tipoMarcacoes { get; set; }
    public int idHistoricoTipo { get; set; }
    public string? data { get; set; }
    public int? qtde { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeQtde { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected flwConfigExcecoes()
    {
        // Required by EF Core
    }

    public flwConfigExcecoes(Guid id) : base(id)
    {
    }
}
