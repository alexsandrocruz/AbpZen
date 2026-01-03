using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Edital.ViewModels;

public class EditEditalViewModel
{
    [Display(Name = "Edital:Objeto")]
    public string? Objeto { get; set; }
    [Display(Name = "Edital:Data")]
    public DateTime? Data { get; set; }
    [Display(Name = "Edital:Valor")]
    public decimal? Valor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
