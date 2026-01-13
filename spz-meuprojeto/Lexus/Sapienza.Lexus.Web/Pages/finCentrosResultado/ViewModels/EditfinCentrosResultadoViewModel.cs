using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finCentrosResultado.ViewModels;

public class EditfinCentrosResultadoViewModel
{
    [Display(Name = "finCentrosResultado:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [Display(Name = "finCentrosResultado:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finCentrosResultado:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finCentrosResultado:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finCentrosResultado:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finCentrosResultado:padrao")]
    public bool? padrao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
