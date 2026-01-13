using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finContasClientes;
using Sapienza.Lexus.finContasClientes.Dtos;

namespace Sapienza.Lexus.Web.Pages.finContasClientes;

public class IndexModel : Sapienza.LexusPageModel
{
    public finContasClientesFilterInput finContasClientesFilter { get; set; }
    
    private readonly IfinContasClientesAppService _finContasClientesAppService;

    public IndexModel(IfinContasClientesAppService finContasClientesAppService)
    {
        _finContasClientesAppService = finContasClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finContasClientesGetListInput input)
    {
        var result = await _finContasClientesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finContasClientesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finContasClientesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:idContaCliente")]
    public int? idContaCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finContasClientes:cor")]
    public string? cor { get; set; }
}
