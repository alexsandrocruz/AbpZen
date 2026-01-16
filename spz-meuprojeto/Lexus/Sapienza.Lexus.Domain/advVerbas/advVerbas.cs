// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advVerbas;

/// <summary>
/// advVerbas entity
/// </summary>
public class advVerbas : FullAuditedAggregateRoot<Guid>
{
    public int? idVerba { get; set; }
    public int idTipo { get; set; }
    public int idProfissional { get; set; }
    public int? idProcesso { get; set; }
    public int? idLancamento { get; set; }
    public double? valor { get; set; }
    public string dataDe { get; set; } = string.Empty;
    public string dataAte { get; set; } = string.Empty;
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public bool? comprovante { get; set; }
    public string? comprovanteArquivo { get; set; }
    public bool? solicitacao { get; set; }
    public bool? aceito { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advProcessos.advProcessos> advVerbases { get; set; } = new List<Sapienza.Lexus.advProcessos.advProcessos>();
    public virtual ICollection<Sapienza.Lexus.advProfissionais.advProfissionais> advVerbasesCollection { get; set; } = new List<Sapienza.Lexus.advProfissionais.advProfissionais>();
    public virtual ICollection<Sapienza.Lexus.advVerTipos.advVerTipos> advVerbasesCollection1 { get; set; } = new List<Sapienza.Lexus.advVerTipos.advVerTipos>();

    protected advVerbas()
    {
        // Required by EF Core
    }

    public advVerbas(Guid id) : base(id)
    {
    }
}
