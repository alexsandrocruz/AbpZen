using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.LawyerSpecialization.ViewModels;

public class EditLawyerSpecializationViewModel
{

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    [Display(Name = "LawyerSpecialization:LawyerId")]
    [SelectItems(nameof(LawyerList))]
    public Guid LawyerId { get; set; }

    public List<SelectListItem> LawyerList { get; set; } = new();
    [Required]
    [Display(Name = "LawyerSpecialization:SpecializationId")]
    [SelectItems(nameof(SpecializationList))]
    public Guid SpecializationId { get; set; }

    public List<SelectListItem> SpecializationList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
