using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Edital.ViewModels;

public class EditEditalViewModel
{
    [Display(Name = "EditalObjeto")]
    public string? Objeto { get; set; }
    [Display(Name = "EditalData")]
    public DateTime? Data { get; set; }
    [Display(Name = "EditalValor")]
    public decimal? Valor { get; set; }
}
