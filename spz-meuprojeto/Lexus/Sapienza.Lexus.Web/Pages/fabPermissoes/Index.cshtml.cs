using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabPermissoes;
using Sapienza.Lexus.fabPermissoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabPermissoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabPermissoesFilterInput fabPermissoesFilter { get; set; }
    
    private readonly IfabPermissoesAppService _fabPermissoesAppService;

    public IndexModel(IfabPermissoesAppService fabPermissoesAppService)
    {
        _fabPermissoesAppService = fabPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabPermissoesGetListInput input)
    {
        var result = await _fabPermissoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabPermissoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabPermissoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoes:idPermissao")]
    public int? idPermissao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoes:idPermissaoTipo")]
    public int? idPermissaoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoes:modulo")]
    public bool? modulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoes:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabPermissoes:varSession")]
    public string? varSession { get; set; }
}
