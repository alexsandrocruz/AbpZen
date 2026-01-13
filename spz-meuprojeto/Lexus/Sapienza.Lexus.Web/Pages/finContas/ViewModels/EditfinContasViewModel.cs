using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finContas.ViewModels;

public class EditfinContasViewModel
{
    [Display(Name = "finContas:idConta")]
    public int? idConta { get; set; }
    [Display(Name = "finContas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finContas:banco")]
    public string? banco { get; set; }
    [Display(Name = "finContas:agencia")]
    public string? agencia { get; set; }
    [Display(Name = "finContas:conta")]
    public string? conta { get; set; }
    [Display(Name = "finContas:favorecido")]
    public string? favorecido { get; set; }
    [Display(Name = "finContas:limite")]
    public double? limite { get; set; }
    [Display(Name = "finContas:padraoFluxo")]
    public bool? padraoFluxo { get; set; }
    [Display(Name = "finContas:considerarIndicador")]
    public bool? considerarIndicador { get; set; }
    [Display(Name = "finContas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finContas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finContas:saldoInicial")]
    public double? saldoInicial { get; set; }
    [Display(Name = "finContas:padrao")]
    public bool? padrao { get; set; }
    [Display(Name = "finContas:codigo")]
    public string? codigo { get; set; }
    [Display(Name = "finContas:cor")]
    public string? cor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
