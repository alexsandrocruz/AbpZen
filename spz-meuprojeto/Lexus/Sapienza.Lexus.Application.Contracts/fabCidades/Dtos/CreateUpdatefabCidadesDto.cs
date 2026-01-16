using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabCidades.Dtos;

[Serializable]
public class CreateUpdatefabCidadesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idCidade { get; set; }
    public string descricao { get; set; }
    public string codigoIBGE { get; set; }
    public int? idEstado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
