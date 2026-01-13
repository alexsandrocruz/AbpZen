using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProSentencas.ViewModels;

public class EditadvProSentencasViewModel
{
    [Display(Name = "advProSentencas:idSentenca")]
    public int? idSentenca { get; set; }
    [Display(Name = "advProSentencas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProSentencas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProSentencas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProSentencas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
