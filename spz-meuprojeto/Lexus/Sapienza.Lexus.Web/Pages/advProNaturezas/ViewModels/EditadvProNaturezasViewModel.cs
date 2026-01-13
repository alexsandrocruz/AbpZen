using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProNaturezas.ViewModels;

public class EditadvProNaturezasViewModel
{
    [Display(Name = "advProNaturezas:idNatureza")]
    public int? idNatureza { get; set; }
    [Display(Name = "advProNaturezas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProNaturezas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProNaturezas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProNaturezas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advProNaturezas:mostraHistoricoNumeros")]
    public bool? mostraHistoricoNumeros { get; set; }
    [Display(Name = "advProNaturezas:recebeAcordo")]
    public bool? recebeAcordo { get; set; }
    [Display(Name = "advProNaturezas:recebeRPV")]
    public bool? recebeRPV { get; set; }
    [Display(Name = "advProNaturezas:recebePrecatorio")]
    public bool? recebePrecatorio { get; set; }
    [Display(Name = "advProNaturezas:recebeAlvara")]
    public bool? recebeAlvara { get; set; }
    [Display(Name = "advProNaturezas:idArea")]
    public int? idArea { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
