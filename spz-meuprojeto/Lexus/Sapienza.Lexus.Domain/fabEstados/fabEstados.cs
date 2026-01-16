// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabEstados;

/// <summary>
/// fabEstados entity
/// </summary>
public class fabEstados : FullAuditedAggregateRoot<Guid>
{
    public int? idEstado { get; set; }
    public string? sigla { get; set; }
    public string? descricao { get; set; }
    public int? idPais { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? fabCidadesId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.fabCidades.fabCidades? fabEstadosNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.fabPaises.fabPaises> fabEstadoses { get; set; } = new List<Sapienza.Lexus.fabPaises.fabPaises>();

    protected fabEstados()
    {
        // Required by EF Core
    }

    public fabEstados(Guid id) : base(id)
    {
    }
}
