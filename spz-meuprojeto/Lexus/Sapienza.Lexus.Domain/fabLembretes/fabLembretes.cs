// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabLembretes;

/// <summary>
/// fabLembretes entity
/// </summary>
public class fabLembretes : FullAuditedAggregateRoot<Guid>
{
    public int? idLembrete { get; set; }
    public int? idUsuario { get; set; }
    public string? mensagem { get; set; }
    public string? destino { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? incluidoPor { get; set; }
    public bool? lido { get; set; }
    public string? tipo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabLembretes()
    {
        // Required by EF Core
    }

    public fabLembretes(Guid id) : base(id)
    {
    }
}
