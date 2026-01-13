using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProOrgaos;
using Sapienza.Lexus.advProOrgaos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProOrgaos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProOrgaosFilterInput advProOrgaosFilter { get; set; }
    
    private readonly IadvProOrgaosAppService _advProOrgaosAppService;

    public IndexModel(IadvProOrgaosAppService advProOrgaosAppService)
    {
        _advProOrgaosAppService = advProOrgaosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProOrgaosGetListInput input)
    {
        var result = await _advProOrgaosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProOrgaosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProOrgaosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProOrgaos:idOrgao")]
    public int? idOrgao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProOrgaos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProOrgaos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProOrgaos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProOrgaos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
