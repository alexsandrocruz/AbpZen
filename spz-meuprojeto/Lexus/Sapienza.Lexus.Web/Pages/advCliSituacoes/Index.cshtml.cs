using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advCliSituacoes;
using Sapienza.Lexus.advCliSituacoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advCliSituacoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advCliSituacoesFilterInput advCliSituacoesFilter { get; set; }
    
    private readonly IadvCliSituacoesAppService _advCliSituacoesAppService;

    public IndexModel(IadvCliSituacoesAppService advCliSituacoesAppService)
    {
        _advCliSituacoesAppService = advCliSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advCliSituacoesGetListInput input)
    {
        var result = await _advCliSituacoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advCliSituacoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advCliSituacoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliSituacoes:idSituacao")]
    public int? idSituacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliSituacoes:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliSituacoes:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliSituacoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advCliSituacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
