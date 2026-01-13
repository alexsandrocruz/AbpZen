using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finCentrosResultado;
using Sapienza.Lexus.finCentrosResultado.Dtos;

namespace Sapienza.Lexus.Web.Pages.finCentrosResultado;

public class IndexModel : Sapienza.LexusPageModel
{
    public finCentrosResultadoFilterInput finCentrosResultadoFilter { get; set; }
    
    private readonly IfinCentrosResultadoAppService _finCentrosResultadoAppService;

    public IndexModel(IfinCentrosResultadoAppService finCentrosResultadoAppService)
    {
        _finCentrosResultadoAppService = finCentrosResultadoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finCentrosResultadoGetListInput input)
    {
        var result = await _finCentrosResultadoAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finCentrosResultadoAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finCentrosResultadoFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finCentrosResultado:padrao")]
    public bool? padrao { get; set; }
}
