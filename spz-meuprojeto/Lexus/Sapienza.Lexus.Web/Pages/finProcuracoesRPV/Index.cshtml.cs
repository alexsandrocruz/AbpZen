using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finProcuracoesRPV;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;

namespace Sapienza.Lexus.Web.Pages.finProcuracoesRPV;

public class IndexModel : Sapienza.LexusPageModel
{
    public finProcuracoesRPVFilterInput finProcuracoesRPVFilter { get; set; }
    
    private readonly IfinProcuracoesRPVAppService _finProcuracoesRPVAppService;

    public IndexModel(IfinProcuracoesRPVAppService finProcuracoesRPVAppService)
    {
        _finProcuracoesRPVAppService = finProcuracoesRPVAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finProcuracoesRPVGetListInput input)
    {
        var result = await _finProcuracoesRPVAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finProcuracoesRPVAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finProcuracoesRPVFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:idProcuracao")]
    public int? idProcuracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:impressa")]
    public bool? impressa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:tsImpressa")]
    public DateTime? tsImpressa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:assinada")]
    public bool? assinada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:tsAssinatura")]
    public DateTime? tsAssinatura { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finProcuracoesRPV:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
