using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesChecklist;
using Sapienza.Lexus.advClientesChecklist.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesChecklist;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesChecklistFilterInput advClientesChecklistFilter { get; set; }
    
    private readonly IadvClientesChecklistAppService _advClientesChecklistAppService;

    public IndexModel(IadvClientesChecklistAppService advClientesChecklistAppService)
    {
        _advClientesChecklistAppService = advClientesChecklistAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesChecklistGetListInput input)
    {
        var result = await _advClientesChecklistAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesChecklistAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesChecklistFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesChecklist:idClienteChecklist")]
    public int? idClienteChecklist { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesChecklist:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesChecklist:idTipoArquivo")]
    public int? idTipoArquivo { get; set; }
}
