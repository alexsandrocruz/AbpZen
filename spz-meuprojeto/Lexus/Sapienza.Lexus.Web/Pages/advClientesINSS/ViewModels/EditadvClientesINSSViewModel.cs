using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesINSS.ViewModels;

public class EditadvClientesINSSViewModel
{
    [Display(Name = "advClientesINSS:idInssAgendado")]
    public int? idInssAgendado { get; set; }
    [Required]
    [Display(Name = "advClientesINSS:idCliente")]
    public int idCliente { get; set; }
    [Display(Name = "advClientesINSS:inssAgendado")]
    public bool? inssAgendado { get; set; }
    [Display(Name = "advClientesINSS:inssData")]
    public string? inssData { get; set; }
    [Display(Name = "advClientesINSS:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [Display(Name = "advClientesINSS:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [Display(Name = "advClientesINSS:inssResultado")]
    public string? inssResultado { get; set; }
    [Required]
    [Display(Name = "advClientesINSS:tsInclusao")]
    public string tsInclusao { get; set; } = string.Empty;
    [Display(Name = "advClientesINSS:tsAlteracao")]
    public string? tsAlteracao { get; set; }
    [Display(Name = "advClientesINSS:inssResultadoIndicadorOculto")]
    public bool? inssResultadoIndicadorOculto { get; set; }
    [Display(Name = "advClientesINSS:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [Display(Name = "advClientesINSS:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [Display(Name = "advClientesINSS:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [Display(Name = "advClientesINSS:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advClientesINSS:dataFinalizacao")]
    public string? dataFinalizacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
