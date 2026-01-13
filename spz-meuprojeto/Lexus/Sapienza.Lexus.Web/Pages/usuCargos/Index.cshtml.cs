using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuCargos;
using Sapienza.Lexus.usuCargos.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuCargos;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuCargosFilterInput usuCargosFilter { get; set; }
    
    private readonly IusuCargosAppService _usuCargosAppService;

    public IndexModel(IusuCargosAppService usuCargosAppService)
    {
        _usuCargosAppService = usuCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuCargosGetListInput input)
    {
        var result = await _usuCargosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuCargosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuCargosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:idCargo")]
    public int? idCargo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuCargos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
