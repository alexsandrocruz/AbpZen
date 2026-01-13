using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabHistoricoTipos.ViewModels;

public class CreatefabHistoricoTiposViewModel
{
    [Display(Name = "fabHistoricoTipos:idHistoricoTipo")]
    public int? idHistoricoTipo { get; set; }
    [Display(Name = "fabHistoricoTipos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabHistoricoTipos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabHistoricoTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabHistoricoTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fabHistoricoTipos:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
