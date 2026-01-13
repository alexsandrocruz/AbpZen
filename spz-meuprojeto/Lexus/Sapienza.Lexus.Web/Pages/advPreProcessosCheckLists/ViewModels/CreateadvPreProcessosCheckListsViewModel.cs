using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreProcessosCheckLists.ViewModels;

public class CreateadvPreProcessosCheckListsViewModel
{
    [Display(Name = "advPreProcessosCheckLists:idPreCheckList")]
    public int? idPreCheckList { get; set; }
    [Display(Name = "advPreProcessosCheckLists:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advPreProcessosCheckLists:idGrupo")]
    public int? idGrupo { get; set; }
    [Display(Name = "advPreProcessosCheckLists:idCheckList")]
    public int? idCheckList { get; set; }
    [Display(Name = "advPreProcessosCheckLists:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPreProcessosCheckLists:grupo")]
    public string? grupo { get; set; }
    [Display(Name = "advPreProcessosCheckLists:item")]
    public string? item { get; set; }
    [Display(Name = "advPreProcessosCheckLists:concluido")]
    public bool? concluido { get; set; }
    [Display(Name = "advPreProcessosCheckLists:tsConclusao")]
    public string? tsConclusao { get; set; }
    [Display(Name = "advPreProcessosCheckLists:idResponsavel")]
    public int? idResponsavel { get; set; }
    [Display(Name = "advPreProcessosCheckLists:ordem")]
    public int? ordem { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
