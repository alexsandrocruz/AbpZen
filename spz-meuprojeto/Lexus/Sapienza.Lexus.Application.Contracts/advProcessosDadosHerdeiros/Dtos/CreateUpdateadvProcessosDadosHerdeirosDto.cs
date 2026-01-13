using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;

[Serializable]
public class CreateUpdateadvProcessosDadosHerdeirosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idHerdeiro { get; set; }
    [Required]
    public int idProcesso { get; set; }
    public int? sequencia { get; set; }
    public int? bancarioBancoId { get; set; }
    public string bancarioTipoConta { get; set; }
    public string bancarioAgencia { get; set; }
    public string bancarioConta { get; set; }
    public string bancarioFavorecido { get; set; }
    public string bancarioCpf { get; set; }
    public double? bancarioPerc { get; set; }
    public double? bancarioTarifa { get; set; }
    public string bancarioTarifaParcelas { get; set; }
    public int? idHonorario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
