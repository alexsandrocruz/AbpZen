// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advTarefasAtualizacoes;

/// <summary>
/// advTarefasAtualizacoes entity
/// </summary>
public class advTarefasAtualizacoes : FullAuditedAggregateRoot<Guid>
{
    public int? idAtualizacaoTarefa { get; set; }
    public int? idTarefa { get; set; }
    public int? idCompromisso { get; set; }
    public string? campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? dadoAnterior { get; set; }
    public Guid? IdentityUserId { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advTarefasAtualizacoes()
    {
        // Required by EF Core
    }

    public advTarefasAtualizacoes(Guid id) : base(id)
    {
    }
}
