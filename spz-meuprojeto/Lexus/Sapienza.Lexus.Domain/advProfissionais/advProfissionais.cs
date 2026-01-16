// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProfissionais;

/// <summary>
/// advProfissionais entity
/// </summary>
public class advProfissionais : FullAuditedAggregateRoot<Guid>
{
    public int? idProfissional { get; set; }
    public Guid IdentityUserId { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advProfissionaisEstadosId { get; set; }
    public Guid? advProfissionaisNaturezasId { get; set; }
    public Guid? advVerbasId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados? advProfissionaisNav { get; set; }
    public virtual Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas? advProfissionaisNav1 { get; set; }
    public virtual Sapienza.Lexus.advVerbas.advVerbas? advProfissionaisNav2 { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProfissionais()
    {
        // Required by EF Core
    }

    public advProfissionais(Guid id) : base(id)
    {
    }
}
