using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.LegalProcess.ViewModels;

public class EditLegalProcessViewModel
{
    [Required]
    [StringLength(32)]
    [Display(Name = "LegalProcess:ProcessNumber")]
    public string ProcessNumber { get; set; } = string.Empty;
    [Required]
    [StringLength(128)]
    [Display(Name = "LegalProcess:Title")]
    public string Title { get; set; } = string.Empty;
    [StringLength(2048)]
    [Display(Name = "LegalProcess:Description")]
    [TextArea(Rows = 3)]
    public string? Description { get; set; }
    [Required]
    [Display(Name = "LegalProcess:DateOpened")]
    public DateTime DateOpened { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    [Display(Name = "LegalProcess:LawyerId")]
    [SelectItems(nameof(LawyerList))]
    public Guid LawyerId { get; set; }

    public List<SelectListItem> LawyerList { get; set; } = new();
    [Required]
    [Display(Name = "LegalProcess:ClientId")]
    [SelectItems(nameof(ClientList))]
    public Guid ClientId { get; set; }

    public List<SelectListItem> ClientList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
