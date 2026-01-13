using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finCentrosCusto.ViewModels;

public class EditfinCentrosCustoViewModel
{
    [Display(Name = "finCentrosCusto:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [Display(Name = "finCentrosCusto:idUnidade")]
    public int? idUnidade { get; set; }
    [Display(Name = "finCentrosCusto:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finCentrosCusto:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finCentrosCusto:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finCentrosCusto:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finCentrosCusto:padrao")]
    public bool? padrao { get; set; }
    [Display(Name = "finCentrosCusto:porcentagemRateio")]
    public double? porcentagemRateio { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
