using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientesHistoricos.Dtos;

[Serializable]
public class CreateUpdateadvClientesHistoricosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idHistorico { get; set; }
    [Required]
    public int idCliente { get; set; }
    public int? idProcesso { get; set; }
    [Required]
    public int idUsuario { get; set; }
    [Required]
    public int idTipoHistorico { get; set; }
    [Required]
    public string data { get; set; } = string.Empty;
    public string hora { get; set; }
    public string ocorrencia { get; set; }
    public DateTime? tsInclusao { get; set; }
    public int? idOportunidade { get; set; }
    public string depto { get; set; }
    public bool? prioritario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
