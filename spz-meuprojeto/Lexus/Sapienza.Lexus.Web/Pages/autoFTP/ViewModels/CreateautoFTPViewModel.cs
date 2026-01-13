using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.autoFTP.ViewModels;

public class CreateautoFTPViewModel
{
    [Display(Name = "autoFTP:id")]
    public int? id { get; set; }
    [Display(Name = "autoFTP:arquivo")]
    public string? arquivo { get; set; }
    [Display(Name = "autoFTP:processado")]
    public bool? processado { get; set; }
    [Display(Name = "autoFTP:tsInclusao")]
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
