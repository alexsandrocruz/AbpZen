using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advTarefasAtualizacoes.ViewModels;

public class CreateadvTarefasAtualizacoesViewModel
{
    [Display(Name = "advTarefasAtualizacoes:idAtualizacaoTarefa")]
    public int? idAtualizacaoTarefa { get; set; }
    [Display(Name = "advTarefasAtualizacoes:idTarefa")]
    public int? idTarefa { get; set; }
    [Display(Name = "advTarefasAtualizacoes:idCompromisso")]
    public int? idCompromisso { get; set; }
    [Display(Name = "advTarefasAtualizacoes:campo")]
    public string? campo { get; set; }
    [Display(Name = "advTarefasAtualizacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advTarefasAtualizacoes:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [Display(Name = "advTarefasAtualizacoes:idUsuario")]
    public int? idUsuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
