using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finPrestacaoContas.ViewModels;

public class EditfinPrestacaoContasViewModel
{
    [Display(Name = "finPrestacaoContas:idPrestacao")]
    public int? idPrestacao { get; set; }
    [Required]
    [Display(Name = "finPrestacaoContas:idLancamento")]
    public int idLancamento { get; set; }
    [Display(Name = "finPrestacaoContas:levantado")]
    public double? levantado { get; set; }
    [Display(Name = "finPrestacaoContas:irpj")]
    public double? irpj { get; set; }
    [Display(Name = "finPrestacaoContas:carta")]
    public double? carta { get; set; }
    [Display(Name = "finPrestacaoContas:honorarios")]
    public double? honorarios { get; set; }
    [Display(Name = "finPrestacaoContas:tarifa")]
    public double? tarifa { get; set; }
    [Display(Name = "finPrestacaoContas:liquidoRecebido")]
    public double? liquidoRecebido { get; set; }
    [Display(Name = "finPrestacaoContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
