using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreLogStatus.ViewModels;

public class EditadvPreLogStatusViewModel
{
    [Display(Name = "advPreLogStatus:idLog")]
    public int? idLog { get; set; }
    [Required]
    [Display(Name = "advPreLogStatus:idProcesso")]
    public int idProcesso { get; set; }
    [Required]
    [Display(Name = "advPreLogStatus:idStatus")]
    public int idStatus { get; set; }
    [Display(Name = "advPreLogStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPreLogStatus:conversao")]
    public bool? conversao { get; set; }
    [Display(Name = "advPreLogStatus:tsConversao")]
    public DateTime? tsConversao { get; set; }
    [Display(Name = "advPreLogStatus:perdido")]
    public bool? perdido { get; set; }
    [Display(Name = "advPreLogStatus:tsPerdido")]
    public DateTime? tsPerdido { get; set; }
    [Display(Name = "advPreLogStatus:diasCorridosDoAnterior")]
    public int? diasCorridosDoAnterior { get; set; }
    [Display(Name = "advPreLogStatus:usuario")]
    public string? usuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
