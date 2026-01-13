using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreMotivosPerda;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreMotivosPerda;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreMotivosPerdaFilterInput advPreMotivosPerdaFilter { get; set; }
    
    private readonly IadvPreMotivosPerdaAppService _advPreMotivosPerdaAppService;

    public IndexModel(IadvPreMotivosPerdaAppService advPreMotivosPerdaAppService)
    {
        _advPreMotivosPerdaAppService = advPreMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreMotivosPerdaGetListInput input)
    {
        var result = await _advPreMotivosPerdaAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreMotivosPerdaAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreMotivosPerdaFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMotivosPerda:idMotivo")]
    public int? idMotivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMotivosPerda:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMotivosPerda:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMotivosPerda:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreMotivosPerda:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
