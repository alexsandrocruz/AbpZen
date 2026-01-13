using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabMotivosPerda;
using Sapienza.Lexus.fabMotivosPerda.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabMotivosPerda;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabMotivosPerdaFilterInput fabMotivosPerdaFilter { get; set; }
    
    private readonly IfabMotivosPerdaAppService _fabMotivosPerdaAppService;

    public IndexModel(IfabMotivosPerdaAppService fabMotivosPerdaAppService)
    {
        _fabMotivosPerdaAppService = fabMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabMotivosPerdaGetListInput input)
    {
        var result = await _fabMotivosPerdaAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabMotivosPerdaAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabMotivosPerdaFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosPerda:idMotivo")]
    public int? idMotivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosPerda:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosPerda:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosPerda:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabMotivosPerda:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
