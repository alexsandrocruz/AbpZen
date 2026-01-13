using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advVerbas;
using Sapienza.Lexus.advVerbas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advVerbas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advVerbasFilterInput advVerbasFilter { get; set; }
    
    private readonly IadvVerbasAppService _advVerbasAppService;

    public IndexModel(IadvVerbasAppService advVerbasAppService)
    {
        _advVerbasAppService = advVerbasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advVerbasGetListInput input)
    {
        var result = await _advVerbasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advVerbasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advVerbasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:idVerba")]
    public int? idVerba { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:idProfissional")]
    public int? idProfissional { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:valor")]
    public double? valor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:dataDe")]
    public string? dataDe { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:dataAte")]
    public string? dataAte { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:comprovante")]
    public bool? comprovante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:comprovanteArquivo")]
    public string? comprovanteArquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:solicitacao")]
    public bool? solicitacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:aceito")]
    public bool? aceito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advVerbas:alteradoPor")]
    public string? alteradoPor { get; set; }
}
