using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabEstados.Dtos;

[Serializable]
public class CreateUpdatefabEstadosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idEstado { get; set; }
    public string sigla { get; set; }
    public string descricao { get; set; }
    public int? idPais { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? fabCidadesId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
