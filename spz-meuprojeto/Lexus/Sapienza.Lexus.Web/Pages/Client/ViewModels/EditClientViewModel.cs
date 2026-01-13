using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.Client.ViewModels;

public class EditClientViewModel
{
    [Required]
    [StringLength(128)]
    [Display(Name = "Client:Name")]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(128)]
    [Display(Name = "Client:Email")]
    public string Email { get; set; } = string.Empty;
    [StringLength(20)]
    [Display(Name = "Client:Phone")]
    public string? Phone { get; set; }
    [Required]
    [StringLength(20)]
    [Display(Name = "Client:CpfCnpj")]
    public string CpfCnpj { get; set; } = string.Empty;

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
