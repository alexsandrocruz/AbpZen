using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finProcuracoesRPV.ViewModels;

public class CreatefinProcuracoesRPVViewModel
{
    [Display(Name = "finProcuracoesRPV:idProcuracao")]
    public int? idProcuracao { get; set; }
    [Display(Name = "finProcuracoesRPV:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "finProcuracoesRPV:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "finProcuracoesRPV:impressa")]
    public bool? impressa { get; set; }
    [Display(Name = "finProcuracoesRPV:tsImpressa")]
    public DateTime? tsImpressa { get; set; }
    [Display(Name = "finProcuracoesRPV:assinada")]
    public bool? assinada { get; set; }
    [Display(Name = "finProcuracoesRPV:tsAssinatura")]
    public DateTime? tsAssinatura { get; set; }
    [Display(Name = "finProcuracoesRPV:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finProcuracoesRPV:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finProcuracoesRPV:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
