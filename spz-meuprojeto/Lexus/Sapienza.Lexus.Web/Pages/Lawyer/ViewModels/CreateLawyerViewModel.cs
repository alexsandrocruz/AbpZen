using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.Web.Pages.Lawyer.ViewModels;

public class CreateLawyerViewModel
{
    [Required]
    [StringLength(128)]
    [Display(Name = "Lawyer:FullName")]
    public string FullName { get; set; } = string.Empty;
    [StringLength(64)]
    [Display(Name = "Lawyer:PreferredName")]
    public string? PreferredName { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateLawyerSpecializationDto> Specializations { get; set; } = new();
}
