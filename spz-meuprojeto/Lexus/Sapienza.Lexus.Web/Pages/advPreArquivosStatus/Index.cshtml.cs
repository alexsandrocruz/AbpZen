using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advPreArquivosStatus;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;

namespace Sapienza.Lexus.Web.Pages.advPreArquivosStatus;

public class IndexModel : Sapienza.LexusPageModel
{
    public advPreArquivosStatusFilterInput advPreArquivosStatusFilter { get; set; }
    
    private readonly IadvPreArquivosStatusAppService _advPreArquivosStatusAppService;

    public IndexModel(IadvPreArquivosStatusAppService advPreArquivosStatusAppService)
    {
        _advPreArquivosStatusAppService = advPreArquivosStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advPreArquivosStatusGetListInput input)
    {
        var result = await _advPreArquivosStatusAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advPreArquivosStatusAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advPreArquivosStatusFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreArquivosStatus:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreArquivosStatus:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreArquivosStatus:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreArquivosStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advPreArquivosStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
