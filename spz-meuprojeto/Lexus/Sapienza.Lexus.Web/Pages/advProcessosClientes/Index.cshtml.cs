using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessosClientes;
using Sapienza.Lexus.advProcessosClientes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessosClientes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosClientesFilterInput advProcessosClientesFilter { get; set; }
    
    private readonly IadvProcessosClientesAppService _advProcessosClientesAppService;

    public IndexModel(IadvProcessosClientesAppService advProcessosClientesAppService)
    {
        _advProcessosClientesAppService = advProcessosClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosClientesGetListInput input)
    {
        var result = await _advProcessosClientesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosClientesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosClientesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosClientes:idProcessoCliente")]
    public int? idProcessoCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosClientes:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosClientes:idCliente")]
    public int? idCliente { get; set; }
}
