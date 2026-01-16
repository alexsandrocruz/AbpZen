using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advAgeTiposCompromissos.Dtos;

[Serializable]
public class CreateUpdateadvAgeTiposCompromissosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idTipoCompromisso { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? recebimentoProcesso { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advCompromissosId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
