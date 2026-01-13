using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros.ViewModels;

public class EditadvProcessosDadosHerdeirosViewModel
{
    [Display(Name = "advProcessosDadosHerdeiros:idHerdeiro")]
    public int? idHerdeiro { get; set; }
    [Required]
    [Display(Name = "advProcessosDadosHerdeiros:idProcesso")]
    public int idProcesso { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:sequencia")]
    public int? sequencia { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioConta")]
    public string? bancarioConta { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTarifa")]
    public double? bancarioTarifa { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:bancarioTarifaParcelas")]
    public string? bancarioTarifaParcelas { get; set; }
    [Display(Name = "advProcessosDadosHerdeiros:idHonorario")]
    public int? idHonorario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
