using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreCheckLists.ViewModels;

public class EditadvPreCheckListsViewModel
{
    [Display(Name = "advPreCheckLists:idCheckList")]
    public int? idCheckList { get; set; }
    [Required]
    [Display(Name = "advPreCheckLists:idGrupo")]
    public int idGrupo { get; set; }
    [Display(Name = "advPreCheckLists:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advPreCheckLists:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advPreCheckLists:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPreCheckLists:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
