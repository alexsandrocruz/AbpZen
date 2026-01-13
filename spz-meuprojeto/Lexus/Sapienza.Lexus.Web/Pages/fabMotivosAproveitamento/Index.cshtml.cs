using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabMotivosAproveitamento;
using Sapienza.Lexus.fabMotivosAproveitamento.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabMotivosAproveitamento;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabMotivosAproveitamentoFilterInput fabMotivosAproveitamentoFilter { get; set; }
    
    private readonly IfabMotivosAproveitamentoAppService _fabMotivosAproveitamentoAppService;

    public IndexModel(IfabMotivosAproveitamentoAppService fabMotivosAproveitamentoAppService)
    {
        _fabMotivosAproveitamentoAppService = fabMotivosAproveitamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabMotivosAproveitamentoGetListInput input)
    {
        var result = await _fabMotivosAproveitamentoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabMotivosAproveitamentoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabMotivosAproveitamentoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosAproveitamento:idMotivo")]
    public int? idMotivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosAproveitamento:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosAproveitamento:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosAproveitamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosAproveitamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
