using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.opoSituacoes;
using Sapienza.Lexus.opoSituacoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.opoSituacoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public opoSituacoesFilterInput opoSituacoesFilter { get; set; }
    
    private readonly IopoSituacoesAppService _opoSituacoesAppService;

    public IndexModel(IopoSituacoesAppService opoSituacoesAppService)
    {
        _opoSituacoesAppService = opoSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(opoSituacoesGetListInput input)
    {
        var result = await _opoSituacoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _opoSituacoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class opoSituacoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:idSituacao")]
    public int? idSituacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:ordem")]
    public int? ordem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoSituacoes:considerarIndicador")]
    public bool? considerarIndicador { get; set; }
}
