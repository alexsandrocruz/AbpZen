// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesINSS;

/// <summary>
/// advClientesINSS entity
/// </summary>
public class advClientesINSS : FullAuditedAggregateRoot<Guid>
{
    public int? idInssAgendado { get; set; }
    public int idCliente { get; set; }
    public bool? inssAgendado { get; set; }
    public string? inssData { get; set; }
    public int? inssIdTipoBeneficio { get; set; }
    public int? inssIdPosto { get; set; }
    public string? inssResultado { get; set; }
    public string tsInclusao { get; set; } = string.Empty;
    public string? tsAlteracao { get; set; }
    public bool? inssResultadoIndicadorOculto { get; set; }
    public int? inssResponsavel { get; set; }
    public string? inssProtocolo { get; set; }
    public int? inssIdUsuarioInclusao { get; set; }
    public int? idStatus { get; set; }
    public string? dataFinalizacao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advClientesINSS()
    {
        // Required by EF Core
    }

    public advClientesINSS(Guid id) : base(id)
    {
    }
}
