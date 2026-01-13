using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.flwConfigExcecoes;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.flwConfigExcecoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public flwConfigExcecoesFilterInput flwConfigExcecoesFilter { get; set; }
    
    private readonly IflwConfigExcecoesAppService _flwConfigExcecoesAppService;

    public IndexModel(IflwConfigExcecoesAppService flwConfigExcecoesAppService)
    {
        _flwConfigExcecoesAppService = flwConfigExcecoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(flwConfigExcecoesGetListInput input)
    {
        var result = await _flwConfigExcecoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _flwConfigExcecoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class flwConfigExcecoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:idConfig")]
    public int? idConfig { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:idHistoricoTipo")]
    public int? idHistoricoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:qtde")]
    public int? qtde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:manhaQtde")]
    public int? manhaQtde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwConfigExcecoes:tardeQtde")]
    public int? tardeQtde { get; set; }
}
