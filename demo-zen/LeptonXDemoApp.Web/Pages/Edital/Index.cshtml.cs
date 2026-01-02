using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.Edital;

public class IndexModel : LeptonXDemoAppPageModel
{
    public EditalFilterInput EditalFilter { get; set; }
    
    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}

public class EditalFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Edital:Objeto")]
    public string? Objeto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Edital:Data")]
    public DateTime? Data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Edital:Valor")]
    public decimal? Valor { get; set; }
}
