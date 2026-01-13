using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finExtrato;
using Sapienza.Lexus.finExtrato.Dtos;

namespace Sapienza.Lexus.Web.Pages.finExtrato;

public class IndexModel : Sapienza.LexusPageModel
{
    public finExtratoFilterInput finExtratoFilter { get; set; }
    
    private readonly IfinExtratoAppService _finExtratoAppService;

    public IndexModel(IfinExtratoAppService finExtratoAppService)
    {
        _finExtratoAppService = finExtratoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finExtratoGetListInput input)
    {
        var result = await _finExtratoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finExtratoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finExtratoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idExtrato")]
    public int? idExtrato { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:transferencia")]
    public bool? transferencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idExtratoRel")]
    public int? idExtratoRel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:credito")]
    public double? credito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:debito")]
    public double? debito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:conferido")]
    public bool? conferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finExtrato:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }
}
