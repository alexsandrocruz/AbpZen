using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesAtualizacoes.Dtos;

[Serializable]
public class CreateUpdateadvClientesAtualizacoesDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idAtualizacao { get; set; }
    [Required]
    public int idCliente { get; set; }
    public string campo { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string dadoAnterior { get; set; }
    public int? idUsuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
