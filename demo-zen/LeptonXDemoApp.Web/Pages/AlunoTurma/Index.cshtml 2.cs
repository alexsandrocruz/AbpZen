using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.AlunoTurma;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Web.Pages.AlunoTurma;

public class IndexModel : LeptonXDemoAppPageModel
{
    public AlunoTurmaFilterInput AlunoTurmaFilter { get; set; }
    
    private readonly IAlunoTurmaAppService _alunoTurmaAppService;

    public IndexModel(IAlunoTurmaAppService alunoTurmaAppService)
    {
        _alunoTurmaAppService = alunoTurmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(AlunoTurmaGetListInput input)
    {
        var result = await _alunoTurmaAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _alunoTurmaAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class AlunoTurmaFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "AlunoTurma:AlunoId")]
    public Guid? AlunoId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "AlunoTurma:TurmaId")]
    public Guid? TurmaId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "AlunoTurma:DataMatricula")]
    public DateTime? DataMatricula { get; set; }
}
