using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuPermissoes.ViewModels;

public class EditusuPermissoesViewModel
{
    [Display(Name = "usuPermissoes:idUsuarioPermissao")]
    public int? idUsuarioPermissao { get; set; }
    [Display(Name = "usuPermissoes:idUsuario")]
    public int? idUsuario { get; set; }
    [Display(Name = "usuPermissoes:idPermissao")]
    public int? idPermissao { get; set; }
    [Display(Name = "usuPermissoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "usuPermissoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
