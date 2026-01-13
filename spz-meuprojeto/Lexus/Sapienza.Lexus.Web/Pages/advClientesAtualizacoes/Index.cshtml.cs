using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesAtualizacoes;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesAtualizacoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesAtualizacoesFilterInput advClientesAtualizacoesFilter { get; set; }
    
    private readonly IadvClientesAtualizacoesAppService _advClientesAtualizacoesAppService;

    public IndexModel(IadvClientesAtualizacoesAppService advClientesAtualizacoesAppService)
    {
        _advClientesAtualizacoesAppService = advClientesAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesAtualizacoesGetListInput input)
    {
        var result = await _advClientesAtualizacoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesAtualizacoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesAtualizacoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:idAtualizacao")]
    public int? idAtualizacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:campo")]
    public string? campo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesAtualizacoes:idUsuario")]
    public int? idUsuario { get; set; }
}
