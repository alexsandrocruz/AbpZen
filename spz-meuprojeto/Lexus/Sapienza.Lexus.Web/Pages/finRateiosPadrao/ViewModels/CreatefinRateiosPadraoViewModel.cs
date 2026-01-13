using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finRateiosPadrao.ViewModels;

public class CreatefinRateiosPadraoViewModel
{
    [Display(Name = "finRateiosPadrao:idPadrao")]
    public int? idPadrao { get; set; }
    [Required]
    [Display(Name = "finRateiosPadrao:idUnidade")]
    public int idUnidade { get; set; }
    [Required]
    [Display(Name = "finRateiosPadrao:idCentroResultado")]
    public int idCentroResultado { get; set; }
    [Required]
    [Display(Name = "finRateiosPadrao:porcentagem")]
    public double porcentagem { get; set; }
    [Display(Name = "finRateiosPadrao:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finRateiosPadrao:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finRateiosPadrao:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
