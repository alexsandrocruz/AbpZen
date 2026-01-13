using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposCompromissos.ViewModels;

public class EditadvAgeTiposCompromissosViewModel
{
    [Display(Name = "advAgeTiposCompromissos:idTipoCompromisso")]
    public int? idTipoCompromisso { get; set; }
    [Display(Name = "advAgeTiposCompromissos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advAgeTiposCompromissos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advAgeTiposCompromissos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advAgeTiposCompromissos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advAgeTiposCompromissos:recebimentoProcesso")]
    public bool? recebimentoProcesso { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
