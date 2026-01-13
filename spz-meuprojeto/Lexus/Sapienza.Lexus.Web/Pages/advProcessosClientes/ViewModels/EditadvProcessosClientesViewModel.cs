using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessosClientes.ViewModels;

public class EditadvProcessosClientesViewModel
{
    [Display(Name = "advProcessosClientes:idProcessoCliente")]
    public int? idProcessoCliente { get; set; }
    [Required]
    [Display(Name = "advProcessosClientes:idProcesso")]
    public int idProcesso { get; set; }
    [Required]
    [Display(Name = "advProcessosClientes:idCliente")]
    public int idCliente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
