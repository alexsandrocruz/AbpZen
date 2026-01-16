// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.flwFollows;

/// <summary>
/// flwFollows entity
/// </summary>
public class flwFollows : FullAuditedAggregateRoot<Guid>
{
    public int? idFollow { get; set; }
    public int idCliente { get; set; }
    public int idAcao { get; set; }
    public int idTipo { get; set; }
    public Guid IdentityUserId { get; set; }
    public string data { get; set; } = string.Empty;
    public int? horario { get; set; }
    public string? comentario { get; set; }
    public bool? finalizado { get; set; }
    public string? dataFinalizacao { get; set; }
    public int? horarioFinalizacao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idOportunidade { get; set; }
    public bool? chegou { get; set; }
    public DateTime? tsChegou { get; set; }
    public bool? naoComparecimento { get; set; }
    public bool? prioridade { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> flwFollowses { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();
    public virtual ICollection<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> flwFollowsesCollection { get; set; } = new List<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>();
    public virtual ICollection<Sapienza.Lexus.flwAcoes.flwAcoes> flwFollowsesCollection1 { get; set; } = new List<Sapienza.Lexus.flwAcoes.flwAcoes>();
    public virtual ICollection<Sapienza.Lexus.opoOportunidades.opoOportunidades> flwFollowsesCollection2 { get; set; } = new List<Sapienza.Lexus.opoOportunidades.opoOportunidades>();

    protected flwFollows()
    {
        // Required by EF Core
    }

    public flwFollows(Guid id) : base(id)
    {
    }
}
