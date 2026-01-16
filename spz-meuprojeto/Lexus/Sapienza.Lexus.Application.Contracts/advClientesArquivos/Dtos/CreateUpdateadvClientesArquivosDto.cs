using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesArquivos.Dtos;

[Serializable]
public class CreateUpdateadvClientesArquivosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idArquivo { get; set; }
    [Required]
    public int idCliente { get; set; }
    [Required]
    public int idTipoArquivo { get; set; }
    public string descricao { get; set; }
    public string arquivo { get; set; }
    public string incluidoPor { get; set; }
    public string alteradoPor { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idProcesso { get; set; }
    public bool? precisaRevisao { get; set; }
    public int? idSolicitante { get; set; }
    public string solicitanteComentario { get; set; }
    public int? idRevisor { get; set; }
    public string revisorComentario { get; set; }
    public int? reprovado { get; set; }
    public bool? pendenteVisualizacaoAprovacao { get; set; }
    public string status { get; set; }
    public bool? autoFTP { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
