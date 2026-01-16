using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.logCampos.Dtos;

[Serializable]
public class CreateUpdatelogCamposDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idLogCampo { get; set; }
    [Required]
    public int idLog { get; set; }
    public string campo { get; set; }
    public string dadoAnterior { get; set; }
    public string dadoNovo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
