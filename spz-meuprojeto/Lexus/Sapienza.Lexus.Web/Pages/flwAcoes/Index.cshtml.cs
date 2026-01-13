using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.flwAcoes;
using Sapienza.Lexus.flwAcoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.flwAcoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public flwAcoesFilterInput flwAcoesFilter { get; set; }
    
    private readonly IflwAcoesAppService _flwAcoesAppService;

    public IndexModel(IflwAcoesAppService flwAcoesAppService)
    {
        _flwAcoesAppService = flwAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(flwAcoesGetListInput input)
    {
        var result = await _flwAcoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _flwAcoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class flwAcoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:idAcao")]
    public int? idAcao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwAcoes:diasReagendamento")]
    public int? diasReagendamento { get; set; }
}
