using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finRateios.ViewModels;

public class CreatefinRateiosViewModel
{
    [Display(Name = "finRateios:idRateio")]
    public int? idRateio { get; set; }
    [Required]
    [Display(Name = "finRateios:idLancamento")]
    public int idLancamento { get; set; }
    [Required]
    [Display(Name = "finRateios:idCentroCusto")]
    public int idCentroCusto { get; set; }
    [Display(Name = "finRateios:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [Display(Name = "finRateios:percentualCC")]
    public double? percentualCC { get; set; }
    [Display(Name = "finRateios:percentualCR")]
    public double? percentualCR { get; set; }
    [Display(Name = "finRateios:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finRateios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finRateios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finRateios:idUnidade")]
    public int? idUnidade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
