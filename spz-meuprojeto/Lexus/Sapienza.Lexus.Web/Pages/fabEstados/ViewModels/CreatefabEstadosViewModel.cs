using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabEstados.ViewModels;

public class CreatefabEstadosViewModel
{
    [Display(Name = "fabEstados:idEstado")]
    public int? idEstado { get; set; }
    [Display(Name = "fabEstados:sigla")]
    public string? sigla { get; set; }
    [Display(Name = "fabEstados:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "fabEstados:idPais")]
    public int? idPais { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
