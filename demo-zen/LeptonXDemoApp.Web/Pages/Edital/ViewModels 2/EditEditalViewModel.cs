using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Edital.ViewModels;

public class EditEditalViewModel
{
    [Display(Name = "EditalObjeto")]
    public stringtrue Objeto { get; set; }false
    [Display(Name = "EditalData")]
    public DateTimetrue Data { get; set; }
    [Display(Name = "EditalValor")]
    public decimaltrue Valor { get; set; }
}
