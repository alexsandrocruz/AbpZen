using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProNaturezas.Dtos;

[Serializable]
public class CreateUpdateadvProNaturezasDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idNatureza { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? mostraHistoricoNumeros { get; set; }
    public bool? recebeAcordo { get; set; }
    public bool? recebeRPV { get; set; }
    public bool? recebePrecatorio { get; set; }
    public bool? recebeAlvara { get; set; }
    public int? idArea { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advProfissionaisNaturezasId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
