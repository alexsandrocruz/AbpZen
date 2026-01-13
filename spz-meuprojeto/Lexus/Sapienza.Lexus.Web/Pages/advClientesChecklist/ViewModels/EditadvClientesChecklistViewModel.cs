using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesChecklist.ViewModels;

public class EditadvClientesChecklistViewModel
{
    [Display(Name = "advClientesChecklist:idClienteChecklist")]
    public int? idClienteChecklist { get; set; }
    [Required]
    [Display(Name = "advClientesChecklist:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "advClientesChecklist:idTipoArquivo")]
    public int idTipoArquivo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
