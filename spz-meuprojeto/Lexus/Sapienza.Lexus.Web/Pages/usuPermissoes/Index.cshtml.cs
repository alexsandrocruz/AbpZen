using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuPermissoes;
using Sapienza.Lexus.usuPermissoes.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuPermissoes;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuPermissoesFilterInput usuPermissoesFilter { get; set; }
    
    private readonly IusuPermissoesAppService _usuPermissoesAppService;

    public IndexModel(IusuPermissoesAppService usuPermissoesAppService)
    {
        _usuPermissoesAppService = usuPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuPermissoesGetListInput input)
    {
        var result = await _usuPermissoesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuPermissoesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuPermissoesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuPermissoes:idUsuarioPermissao")]
    public int? idUsuarioPermissao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuPermissoes:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuPermissoes:idPermissao")]
    public int? idPermissao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuPermissoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuPermissoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
