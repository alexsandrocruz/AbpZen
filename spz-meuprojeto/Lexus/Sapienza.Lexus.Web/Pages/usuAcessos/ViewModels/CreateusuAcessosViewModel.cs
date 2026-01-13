using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuAcessos.ViewModels;

public class CreateusuAcessosViewModel
{
    [Display(Name = "usuAcessos:idAcesso")]
    public int? idAcesso { get; set; }
    [Required]
    [Display(Name = "usuAcessos:idUsuario")]
    public int idUsuario { get; set; }
    [Display(Name = "usuAcessos:data")]
    public DateTime? data { get; set; }
    [Display(Name = "usuAcessos:ip")]
    public string? ip { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
