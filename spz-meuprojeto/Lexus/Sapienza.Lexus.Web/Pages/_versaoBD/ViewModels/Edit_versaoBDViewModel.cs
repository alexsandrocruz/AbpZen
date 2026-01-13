using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages._versaoBD.ViewModels;

public class Edit_versaoBDViewModel
{
    [Display(Name = "_versaoBD:id")]
    public int? id { get; set; }
    [Display(Name = "_versaoBD:arquivo")]
    public string? arquivo { get; set; }
    [Display(Name = "_versaoBD:dataAplicacao")]
    public DateTime? dataAplicacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
