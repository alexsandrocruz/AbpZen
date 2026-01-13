using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabCidades.ViewModels;

public class CreatefabCidadesViewModel
{
    [Display(Name = "fabCidades:idCidade")]
    public int? idCidade { get; set; }
    [Display(Name = "fabCidades:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "fabCidades:codigoIBGE")]
    public string? codigoIBGE { get; set; }
    [Display(Name = "fabCidades:idEstado")]
    public int? idEstado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
