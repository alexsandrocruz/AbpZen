// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesAtualizacoes;

/// <summary>
/// advClientesAtualizacoes entity
/// </summary>
public class advClientesAtualizacoes : FullAuditedAggregateRoot<Guid>
{
    public int? idAtualizacao { get; set; }
    public int idCliente { get; set; }
    public string? campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? dadoAnterior { get; set; }
    public Guid? IdentityUserId { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> advClientesAtualizacoeses { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();

    protected advClientesAtualizacoes()
    {
        // Required by EF Core
    }

    public advClientesAtualizacoes(Guid id) : base(id)
    {
    }
}
