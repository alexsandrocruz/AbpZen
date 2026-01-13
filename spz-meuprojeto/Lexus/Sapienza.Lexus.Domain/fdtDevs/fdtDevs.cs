// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fdtDevs;

/// <summary>
/// fdtDevs entity
/// </summary>
public class fdtDevs : FullAuditedAggregateRoot<Guid>
{
    public int? idDev { get; set; }
    public string? pacote { get; set; }
    public string? descricao { get; set; }
    public bool? pendente { get; set; }
    public bool? aprovado { get; set; }
    public bool? reprovado { get; set; }
    public bool? finalizado { get; set; }
    public string? comentariosRevisor { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? alteradoPor { get; set; }
    public bool? ativo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fdtDevs()
    {
        // Required by EF Core
    }

    public fdtDevs(Guid id) : base(id)
    {
    }
}
