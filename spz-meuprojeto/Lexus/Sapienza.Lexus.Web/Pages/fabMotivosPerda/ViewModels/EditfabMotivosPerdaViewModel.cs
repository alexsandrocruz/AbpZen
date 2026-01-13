using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabMotivosPerda.ViewModels;

public class EditfabMotivosPerdaViewModel
{
    [Display(Name = "fabMotivosPerda:idMotivo")]
    public int? idMotivo { get; set; }
    [Display(Name = "fabMotivosPerda:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabMotivosPerda:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabMotivosPerda:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabMotivosPerda:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
