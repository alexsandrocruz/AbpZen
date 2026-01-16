using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advRevisaoDocumentos.Dtos;

[Serializable]
public class CreateUpdateadvRevisaoDocumentosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idRevisao { get; set; }
    [Required]
    public int idUsuarioSolicitante { get; set; }
    [Required]
    public int idUsuarioRevisor { get; set; }
    public string localRede { get; set; }
    public bool? pendente { get; set; }
    public bool? aprovado { get; set; }
    public bool? reprovado { get; set; }
    public bool? finalizado { get; set; }
    public string comentariosSolicitante { get; set; }
    public string comentariosRevisor { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string alteradoPor { get; set; }
    public bool? ativo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
