// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliLog;

/// <summary>
/// advCliLog entity
/// </summary>
public class advCliLog : FullAuditedAggregateRoot<Guid>
{
    public int? idLog { get; set; }
    public int idCliente { get; set; }
    public int idUsuario { get; set; }
    public int acao { get; set; }
    public int idArea { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idResponsavel { get; set; }
    public DateTime? dataAgendamento { get; set; }
    public int? inssIdTipoBeneficio { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliLog()
    {
        // Required by EF Core
    }

    public advCliLog(Guid id) : base(id)
    {
    }
}
