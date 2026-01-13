using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabLembretes;
using Sapienza.Lexus.fabLembretes.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabLembretes;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabLembretesFilterInput fabLembretesFilter { get; set; }
    
    private readonly IfabLembretesAppService _fabLembretesAppService;

    public IndexModel(IfabLembretesAppService fabLembretesAppService)
    {
        _fabLembretesAppService = fabLembretesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabLembretesGetListInput input)
    {
        var result = await _fabLembretesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabLembretesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabLembretesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:idLembrete")]
    public int? idLembrete { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:mensagem")]
    public string? mensagem { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:destino")]
    public string? destino { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:lido")]
    public bool? lido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabLembretes:tipo")]
    public string? tipo { get; set; }
}
