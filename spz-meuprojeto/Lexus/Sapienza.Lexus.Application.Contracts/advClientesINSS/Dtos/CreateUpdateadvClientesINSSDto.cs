using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesINSS.Dtos;

[Serializable]
public class CreateUpdateadvClientesINSSDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idInssAgendado { get; set; }
    [Required]
    public int idCliente { get; set; }
    public bool? inssAgendado { get; set; }
    public string inssData { get; set; }
    public int? inssIdTipoBeneficio { get; set; }
    public int? inssIdPosto { get; set; }
    public string inssResultado { get; set; }
    [Required]
    public string tsInclusao { get; set; } = string.Empty;
    public string tsAlteracao { get; set; }
    public bool? inssResultadoIndicadorOculto { get; set; }
    public int? inssResponsavel { get; set; }
    public string inssProtocolo { get; set; }
    public int? inssIdUsuarioInclusao { get; set; }
    public int? idStatus { get; set; }
    public string dataFinalizacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
