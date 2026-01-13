using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProSentencas;
using Sapienza.Lexus.advProSentencas.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProSentencas;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProSentencasFilterInput advProSentencasFilter { get; set; }
    
    private readonly IadvProSentencasAppService _advProSentencasAppService;

    public IndexModel(IadvProSentencasAppService advProSentencasAppService)
    {
        _advProSentencasAppService = advProSentencasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProSentencasGetListInput input)
    {
        var result = await _advProSentencasAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProSentencasAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProSentencasFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProSentencas:idSentenca")]
    public int? idSentenca { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProSentencas:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProSentencas:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProSentencas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProSentencas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
}
