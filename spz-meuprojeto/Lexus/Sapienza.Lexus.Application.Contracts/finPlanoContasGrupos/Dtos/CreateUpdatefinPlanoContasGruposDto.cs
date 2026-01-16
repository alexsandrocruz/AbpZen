using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finPlanoContasGrupos.Dtos;

[Serializable]
public class CreateUpdatefinPlanoContasGruposDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idGrupo { get; set; }
    public string titulo { get; set; }
    public string tipo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idGrupoDRE { get; set; }
    public int? ordem { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? finPlanoContasId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
