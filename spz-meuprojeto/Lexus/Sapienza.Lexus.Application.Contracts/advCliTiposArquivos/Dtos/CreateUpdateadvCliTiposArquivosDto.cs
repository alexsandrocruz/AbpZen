using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advCliTiposArquivos.Dtos;

[Serializable]
public class CreateUpdateadvCliTiposArquivosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idTipoArquivo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string pasta { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? advClientesArquivosId { get; set; }
    public Guid? advClientesChecklistId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
