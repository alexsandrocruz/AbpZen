using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.flwConfigExcecoes.Dtos;

[Serializable]
public class CreateUpdateflwConfigExcecoesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idConfig { get; set; }
    public string tipoMarcacoes { get; set; }
    [Required]
    public int idHistoricoTipo { get; set; }
    public string data { get; set; }
    public int? qtde { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeQtde { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
