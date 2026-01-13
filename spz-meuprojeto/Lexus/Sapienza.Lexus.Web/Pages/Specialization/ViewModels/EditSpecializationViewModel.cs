using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.Web.Pages.Specialization.ViewModels;

public class EditSpecializationViewModel
{
    [Required]
    [StringLength(64)]
    [Display(Name = "Specialization:Name")]
    public string Name { get; set; } = string.Empty;
    [StringLength(512)]
    [Display(Name = "Specialization:Description")]
    [TextArea(Rows = 3)]
    public string? Description { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateLawyerSpecializationDto> Lawyers { get; set; } = new();
}
