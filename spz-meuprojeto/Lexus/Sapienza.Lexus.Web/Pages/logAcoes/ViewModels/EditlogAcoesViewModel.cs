using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.logAcoes.ViewModels;

public class EditlogAcoesViewModel
{
    [Display(Name = "logAcoes:idLog")]
    public int? idLog { get; set; }
    [Display(Name = "logAcoes:area")]
    public string? area { get; set; }
    [Display(Name = "logAcoes:acao")]
    public string? acao { get; set; }
    [Display(Name = "logAcoes:usuario")]
    public string? usuario { get; set; }
    [Display(Name = "logAcoes:motivo")]
    public string? motivo { get; set; }
    [Display(Name = "logAcoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "logAcoes:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "logAcoes:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "logAcoes:idCompromisso")]
    public int? idCompromisso { get; set; }
    [Display(Name = "logAcoes:idTarefa")]
    public int? idTarefa { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
