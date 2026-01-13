using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finRecibos.ViewModels;

public class CreatefinRecibosViewModel
{
    [Display(Name = "finRecibos:idRecibo")]
    public int? idRecibo { get; set; }
    [Display(Name = "finRecibos:idLancamento")]
    public int? idLancamento { get; set; }
    [Display(Name = "finRecibos:numero")]
    public int? numero { get; set; }
    [Display(Name = "finRecibos:referente")]
    public string? referente { get; set; }
    [Display(Name = "finRecibos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finRecibos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finRecibos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
