using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabLembretes.ViewModels;

public class CreatefabLembretesViewModel
{
    [Display(Name = "fabLembretes:idLembrete")]
    public int? idLembrete { get; set; }
    [Display(Name = "fabLembretes:idUsuario")]
    public int? idUsuario { get; set; }
    [Display(Name = "fabLembretes:mensagem")]
    public string? mensagem { get; set; }
    [Display(Name = "fabLembretes:destino")]
    public string? destino { get; set; }
    [Display(Name = "fabLembretes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabLembretes:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "fabLembretes:lido")]
    public bool? lido { get; set; }
    [Display(Name = "fabLembretes:tipo")]
    public string? tipo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
