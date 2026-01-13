using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreArquivosStatus.ViewModels;

public class CreateadvPreArquivosStatusViewModel
{
    [Display(Name = "advPreArquivosStatus:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advPreArquivosStatus:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advPreArquivosStatus:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advPreArquivosStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPreArquivosStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
