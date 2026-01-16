using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.fabHistoricoTipos.Dtos;

[Serializable]
public class CreateUpdatefabHistoricoTiposDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idHistoricoTipo { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string tipoMarcacoes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? flwConfigExcecoesId { get; set; }
    public Guid? flwGradeHorariosId { get; set; }
    public Guid? flwFollowsId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
