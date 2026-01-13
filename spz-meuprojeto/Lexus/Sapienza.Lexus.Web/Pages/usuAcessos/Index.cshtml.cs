using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuAcessos;
using Sapienza.Lexus.usuAcessos.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuAcessos;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuAcessosFilterInput usuAcessosFilter { get; set; }
    
    private readonly IusuAcessosAppService _usuAcessosAppService;

    public IndexModel(IusuAcessosAppService usuAcessosAppService)
    {
        _usuAcessosAppService = usuAcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuAcessosGetListInput input)
    {
        var result = await _usuAcessosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuAcessosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuAcessosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAcessos:idAcesso")]
    public int? idAcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAcessos:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAcessos:data")]
    public DateTime? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuAcessos:ip")]
    public string? ip { get; set; }
}
