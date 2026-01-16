// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.opoOportunidades;

/// <summary>
/// opoOportunidades entity
/// </summary>
public class opoOportunidades : FullAuditedAggregateRoot<Guid>
{
    public int? idOportunidade { get; set; }
    public int idCliente { get; set; }
    public Guid IdentityUserId { get; set; }
    public int idTipo { get; set; }
    public int idSituacao { get; set; }
    public string? titulo { get; set; }
    public string? numero { get; set; }
    public string? dataInicio { get; set; }
    public string? dataEstimada { get; set; }
    public double? valorEstimado { get; set; }
    public string? comentario { get; set; }
    public bool? aproveitada { get; set; }
    public string? aproveitadaData { get; set; }
    public bool? cancelada { get; set; }
    public string? canceladaMotivo { get; set; }
    public string? canceladaData { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? indicadorCanceladoVisto { get; set; }
    public double? valorEstimadoMensal { get; set; }
    public bool? deProcesso { get; set; }
    public string? aproveitadaMotivo { get; set; }
    public int? numeroProcesso { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? opoOrcamentosId { get; set; }
    public Guid flwFollowsId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.opoOrcamentos.opoOrcamentos? opoOportunidadesNav { get; set; }
    public virtual Sapienza.Lexus.flwFollows.flwFollows opoOportunidadesNav1 { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> opoOportunidadeses { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();
    public virtual ICollection<Sapienza.Lexus.opoTipos.opoTipos> opoOportunidadesesCollection { get; set; } = new List<Sapienza.Lexus.opoTipos.opoTipos>();
    public virtual ICollection<Sapienza.Lexus.opoSituacoes.opoSituacoes> opoOportunidadesesCollection1 { get; set; } = new List<Sapienza.Lexus.opoSituacoes.opoSituacoes>();

    protected opoOportunidades()
    {
        // Required by EF Core
    }

    public opoOportunidades(Guid id) : base(id)
    {
    }
}
