using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.Client;
using Sapienza.Lexus.Client.Dtos;

namespace Sapienza.Lexus.Web.Pages.Client;

public class IndexModel : Sapienza.LexusPageModel
{
    public ClientFilterInput ClientFilter { get; set; }
    
    private readonly IClientAppService _clientAppService;

    public IndexModel(IClientAppService clientAppService)
    {
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(ClientGetListInput input)
    {
        var result = await _clientAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _clientAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class ClientFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Client:Name")]
    public string? Name { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Client:Email")]
    public string? Email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Client:Phone")]
    public string? Phone { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Client:CpfCnpj")]
    public string? CpfCnpj { get; set; }
}
