using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finContasClientes.ViewModels;

public class CreatefinContasClientesViewModel
{
    [Display(Name = "finContasClientes:idContaCliente")]
    public int? idContaCliente { get; set; }
    [Display(Name = "finContasClientes:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finContasClientes:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finContasClientes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finContasClientes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finContasClientes:cor")]
    public string? cor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
