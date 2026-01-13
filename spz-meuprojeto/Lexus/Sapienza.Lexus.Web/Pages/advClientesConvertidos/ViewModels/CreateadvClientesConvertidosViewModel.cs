using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesConvertidos.ViewModels;

public class CreateadvClientesConvertidosViewModel
{
    [Display(Name = "advClientesConvertidos:idRegistro")]
    public int? idRegistro { get; set; }
    [Required]
    [Display(Name = "advClientesConvertidos:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "advClientesConvertidos:data")]
    public string data { get; set; } = string.Empty;
    [Display(Name = "advClientesConvertidos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientesConvertidos:convertidoPor")]
    public string? convertidoPor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
