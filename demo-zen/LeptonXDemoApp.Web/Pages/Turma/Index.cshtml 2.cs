using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Turma;
using LeptonXDemoApp.Turma.Dtos;

namespace LeptonXDemoApp.Web.Pages.Turma;

public class IndexModel : LeptonXDemoAppPageModel
{
    public TurmaFilterInput TurmaFilter { get; set; }
    
    private readonly ITurmaAppService _turmaAppService;

    public IndexModel(ITurmaAppService turmaAppService)
    {
        _turmaAppService = turmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(TurmaGetListInput input)
    {
        var result = await _turmaAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _turmaAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class TurmaFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Turma:Nome")]
    public string? Nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Turma:Codigo")]
    public string? Codigo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Turma:AnoLetivo")]
    public int? AnoLetivo { get; set; }
}
