using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.logCampos.ViewModels;

public class CreatelogCamposViewModel
{
    [Display(Name = "logCampos:idLogCampo")]
    public int? idLogCampo { get; set; }
    [Required]
    [Display(Name = "logCampos:idLog")]
    public int idLog { get; set; }
    [Display(Name = "logCampos:campo")]
    public string? campo { get; set; }
    [Display(Name = "logCampos:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [Display(Name = "logCampos:dadoNovo")]
    public string? dadoNovo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
