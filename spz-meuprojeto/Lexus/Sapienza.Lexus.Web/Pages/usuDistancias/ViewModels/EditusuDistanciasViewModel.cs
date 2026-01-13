using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuDistancias.ViewModels;

public class EditusuDistanciasViewModel
{
    [Display(Name = "usuDistancias:idDistancia")]
    public int? idDistancia { get; set; }
    [Required]
    [Display(Name = "usuDistancias:idUsuario")]
    public int idUsuario { get; set; }
    [Display(Name = "usuDistancias:estado")]
    public string? estado { get; set; }
    [Display(Name = "usuDistancias:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "usuDistancias:km")]
    public int? km { get; set; }
    [Display(Name = "usuDistancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
