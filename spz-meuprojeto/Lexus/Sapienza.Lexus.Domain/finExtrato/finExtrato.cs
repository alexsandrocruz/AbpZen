// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finExtrato;

/// <summary>
/// finExtrato entity
/// </summary>
public class finExtrato : FullAuditedAggregateRoot<Guid>
{
    public int? idExtrato { get; set; }
    public int idConta { get; set; }
    public int? idLancamento { get; set; }
    public bool? transferencia { get; set; }
    public int? idExtratoRel { get; set; }
    public string? data { get; set; }
    public string? descricao { get; set; }
    public double? credito { get; set; }
    public double? debito { get; set; }
    public bool? conferido { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUsuarioInclusao { get; set; }
    public int? idUsuarioAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.finContas.finContas> finExtratos { get; set; } = new List<Sapienza.Lexus.finContas.finContas>();

    protected finExtrato()
    {
        // Required by EF Core
    }

    public finExtrato(Guid id) : base(id)
    {
    }
}
