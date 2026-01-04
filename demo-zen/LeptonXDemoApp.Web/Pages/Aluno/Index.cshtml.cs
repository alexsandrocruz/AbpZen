using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Aluno;
using LeptonXDemoApp.Aluno.Dtos;

namespace LeptonXDemoApp.Web.Pages.Aluno;

public class IndexModel : LeptonXDemoAppPageModel
{
    public AlunoFilterInput AlunoFilter { get; set; }
    
    private readonly IAlunoAppService _alunoAppService;

    public IndexModel(IAlunoAppService alunoAppService)
    {
        _alunoAppService = alunoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(AlunoGetListInput input)
    {
        var result = await _alunoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _alunoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class AlunoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Aluno:Nome")]
    public string? Nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Aluno:Email")]
    public string? Email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Aluno:Matricula")]
    public string? Matricula { get; set; }
}
