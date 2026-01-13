using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCompromissos.ViewModels;

public class CreateadvCompromissosViewModel
{
    [Display(Name = "advCompromissos:idCompromisso")]
    public int? idCompromisso { get; set; }
    [Required]
    [Display(Name = "advCompromissos:idTipoCompromisso")]
    public int idTipoCompromisso { get; set; }
    [Display(Name = "advCompromissos:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advCompromissos:dataPublicacao")]
    public string? dataPublicacao { get; set; }
    [Display(Name = "advCompromissos:dataPrazoInterno")]
    public string? dataPrazoInterno { get; set; }
    [Display(Name = "advCompromissos:dataPrazoFatal")]
    public string? dataPrazoFatal { get; set; }
    [Display(Name = "advCompromissos:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "advCompromissos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCompromissos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCompromissos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advCompromissos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advCompromissos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "advCompromissos:idAgendamentoINSS")]
    public int? idAgendamentoINSS { get; set; }
    [Display(Name = "advCompromissos:pauta")]
    public bool? pauta { get; set; }
    [Display(Name = "advCompromissos:pautaIdUsuarioResp")]
    public int? pautaIdUsuarioResp { get; set; }
    [Display(Name = "advCompromissos:pautaRespAceite")]
    public bool? pautaRespAceite { get; set; }
    [Display(Name = "advCompromissos:horarioInicial")]
    public int? horarioInicial { get; set; }
    [Display(Name = "advCompromissos:horarioFinal")]
    public int? horarioFinal { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
