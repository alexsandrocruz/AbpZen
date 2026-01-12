using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Edital;
using LeptonXDemoApp.Edital.Dtos;

namespace LeptonXDemoApp.Web.Pages.Edital;

public class IndexModel : LeptonXDemoAppPageModel
{
    public EditalFilterInput EditalFilter { get; set; }
    
    private readonly IEditalAppService _editalAppService;

    public IndexModel(IEditalAppService editalAppService)
    {
        _editalAppService = editalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(EditalGetListInput input)
    {
        var result = await _editalAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _editalAppService.DeleteAsync(id);
        return new NoContentResult();
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
