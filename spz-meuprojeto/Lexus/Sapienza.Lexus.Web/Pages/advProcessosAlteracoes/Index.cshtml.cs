using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessosAlteracoes;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessosAlteracoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosAlteracoesFilterInput advProcessosAlteracoesFilter { get; set; }
    
    private readonly IadvProcessosAlteracoesAppService _advProcessosAlteracoesAppService;

    public IndexModel(IadvProcessosAlteracoesAppService advProcessosAlteracoesAppService)
    {
        _advProcessosAlteracoesAppService = advProcessosAlteracoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosAlteracoesGetListInput input)
    {
        var result = await _advProcessosAlteracoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosAlteracoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosAlteracoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosAlteracoes:idProcessoAlteracao")]
    public int? idProcessoAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosAlteracoes:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosAlteracoes:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosAlteracoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosAlteracoes:texto")]
    public string? texto { get; set; }
}
