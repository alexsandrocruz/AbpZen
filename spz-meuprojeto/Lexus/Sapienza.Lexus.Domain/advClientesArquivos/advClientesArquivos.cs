// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesArquivos;

/// <summary>
/// advClientesArquivos entity
/// </summary>
public class advClientesArquivos : FullAuditedAggregateRoot<Guid>
{
    public int? idArquivo { get; set; }
    public int idCliente { get; set; }
    public int idTipoArquivo { get; set; }
    public string? descricao { get; set; }
    public string? arquivo { get; set; }
    public string? incluidoPor { get; set; }
    public string? alteradoPor { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idProcesso { get; set; }
    public bool? precisaRevisao { get; set; }
    public int? idSolicitante { get; set; }
    public string? solicitanteComentario { get; set; }
    public int? idRevisor { get; set; }
    public string? revisorComentario { get; set; }
    public int? reprovado { get; set; }
    public bool? pendenteVisualizacaoAprovacao { get; set; }
    public string? status { get; set; }
    public bool? autoFTP { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos> advClientesArquivoses { get; set; } = new List<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos>();
    public virtual ICollection<Sapienza.Lexus.advClientes.advClientes> advClientesArquivosesCollection { get; set; } = new List<Sapienza.Lexus.advClientes.advClientes>();

    protected advClientesArquivos()
    {
        // Required by EF Core
    }

    public advClientesArquivos(Guid id) : base(id)
    {
    }
}
