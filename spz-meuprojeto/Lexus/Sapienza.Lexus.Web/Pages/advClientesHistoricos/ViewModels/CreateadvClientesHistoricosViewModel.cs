using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesHistoricos.ViewModels;

public class CreateadvClientesHistoricosViewModel
{
    [Display(Name = "advClientesHistoricos:idHistorico")]
    public int? idHistorico { get; set; }
    [Required]
    [Display(Name = "advClientesHistoricos:idCliente")]
    public int idCliente { get; set; }
    [Display(Name = "advClientesHistoricos:idProcesso")]
    public int? idProcesso { get; set; }
    [Required]
    [Display(Name = "advClientesHistoricos:idUsuario")]
    public int idUsuario { get; set; }
    [Required]
    [Display(Name = "advClientesHistoricos:idTipoHistorico")]
    public int idTipoHistorico { get; set; }
    [Required]
    [Display(Name = "advClientesHistoricos:data")]
    public string data { get; set; } = string.Empty;
    [Display(Name = "advClientesHistoricos:hora")]
    public string? hora { get; set; }
    [Display(Name = "advClientesHistoricos:ocorrencia")]
    public string? ocorrencia { get; set; }
    [Display(Name = "advClientesHistoricos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientesHistoricos:idOportunidade")]
    public int? idOportunidade { get; set; }
    [Display(Name = "advClientesHistoricos:depto")]
    public string? depto { get; set; }
    [Display(Name = "advClientesHistoricos:prioritario")]
    public bool? prioritario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
