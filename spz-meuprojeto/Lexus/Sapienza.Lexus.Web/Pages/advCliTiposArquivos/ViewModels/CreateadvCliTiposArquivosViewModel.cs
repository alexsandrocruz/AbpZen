using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliTiposArquivos.ViewModels;

public class CreateadvCliTiposArquivosViewModel
{
    [Display(Name = "advCliTiposArquivos:idTipoArquivo")]
    public int? idTipoArquivo { get; set; }
    [Display(Name = "advCliTiposArquivos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliTiposArquivos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliTiposArquivos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliTiposArquivos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advCliTiposArquivos:pasta")]
    public string? pasta { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
