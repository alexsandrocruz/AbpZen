using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabPermissoesTipos;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabPermissoesTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabPermissoesTiposFilterInput fabPermissoesTiposFilter { get; set; }
    
    private readonly IfabPermissoesTiposAppService _fabPermissoesTiposAppService;

    public IndexModel(IfabPermissoesTiposAppService fabPermissoesTiposAppService)
    {
        _fabPermissoesTiposAppService = fabPermissoesTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabPermissoesTiposGetListInput input)
    {
        var result = await _fabPermissoesTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabPermissoesTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabPermissoesTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoesTipos:idPermissaoTipo")]
    public int? idPermissaoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoesTipos:descricao")]
    public string? descricao { get; set; }
}
