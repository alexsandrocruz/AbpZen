// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advRevisaoDocumentos;

/// <summary>
/// advRevisaoDocumentos entity
/// </summary>
public class advRevisaoDocumentos : FullAuditedAggregateRoot<Guid>
{
    public int? idRevisao { get; set; }
    public int idUsuarioSolicitante { get; set; }
    public int idUsuarioRevisor { get; set; }
    public string? localRede { get; set; }
    public bool? pendente { get; set; }
    public bool? aprovado { get; set; }
    public bool? reprovado { get; set; }
    public bool? finalizado { get; set; }
    public string? comentariosSolicitante { get; set; }
    public string? comentariosRevisor { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? alteradoPor { get; set; }
    public bool? ativo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advRevisaoDocumentos()
    {
        // Required by EF Core
    }

    public advRevisaoDocumentos(Guid id) : base(id)
    {
    }
}
