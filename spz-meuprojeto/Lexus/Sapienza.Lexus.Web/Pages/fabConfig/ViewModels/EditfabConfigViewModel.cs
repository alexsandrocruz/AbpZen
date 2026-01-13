using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabConfig.ViewModels;

public class EditfabConfigViewModel
{
    [Display(Name = "fabConfig:idConfig")]
    public int? idConfig { get; set; }
    [Display(Name = "fabConfig:imagemLogin")]
    public string? imagemLogin { get; set; }
    [Display(Name = "fabConfig:imagemLoginCentral")]
    public string? imagemLoginCentral { get; set; }
    [Display(Name = "fabConfig:imagemLoginTickets")]
    public string? imagemLoginTickets { get; set; }
    [Display(Name = "fabConfig:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fabConfig:precoCombustivel")]
    public double? precoCombustivel { get; set; }
    [Display(Name = "fabConfig:dataBloqueioFinanceiro")]
    public string? dataBloqueioFinanceiro { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
