using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advFornecedores;
using Sapienza.Lexus.advFornecedores.Dtos;

namespace Sapienza.Lexus.Web.Pages.advFornecedores;

public class IndexModel : Sapienza.LexusPageModel
{
    public advFornecedoresFilterInput advFornecedoresFilter { get; set; }
    
    private readonly IadvFornecedoresAppService _advFornecedoresAppService;

    public IndexModel(IadvFornecedoresAppService advFornecedoresAppService)
    {
        _advFornecedoresAppService = advFornecedoresAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advFornecedoresGetListInput input)
    {
        var result = await _advFornecedoresAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advFornecedoresAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advFornecedoresFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:idFornecedor")]
    public int? idFornecedor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:apelido")]
    public string? apelido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:nome")]
    public string? nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:email")]
    public string? email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:telCelular")]
    public string? telCelular { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:telCelularObs")]
    public string? telCelularObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:telFixo")]
    public string? telFixo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:telFixoObs")]
    public string? telFixoObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:endereco")]
    public string? endereco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:complemento")]
    public string? complemento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:bairro")]
    public string? bairro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:cep")]
    public string? cep { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:observacoes")]
    public string? observacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:parceiroEmProcesso")]
    public bool? parceiroEmProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:parceiroEmProcessoPerc")]
    public double? parceiroEmProcessoPerc { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:idProfissional")]
    public int? idProfissional { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advFornecedores:foto")]
    public string? foto { get; set; }
}
