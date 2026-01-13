using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advTarefasAtualizacoes;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advTarefasAtualizacoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advTarefasAtualizacoesFilterInput advTarefasAtualizacoesFilter { get; set; }
    
    private readonly IadvTarefasAtualizacoesAppService _advTarefasAtualizacoesAppService;

    public IndexModel(IadvTarefasAtualizacoesAppService advTarefasAtualizacoesAppService)
    {
        _advTarefasAtualizacoesAppService = advTarefasAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advTarefasAtualizacoesGetListInput input)
    {
        var result = await _advTarefasAtualizacoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advTarefasAtualizacoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advTarefasAtualizacoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:idAtualizacaoTarefa")]
    public int? idAtualizacaoTarefa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:idTarefa")]
    public int? idTarefa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:idCompromisso")]
    public int? idCompromisso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:campo")]
    public string? campo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advTarefasAtualizacoes:idUsuario")]
    public int? idUsuario { get; set; }
}
