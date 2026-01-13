using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesINSS;
using Sapienza.Lexus.advClientesINSS.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesINSS;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesINSSFilterInput advClientesINSSFilter { get; set; }
    
    private readonly IadvClientesINSSAppService _advClientesINSSAppService;

    public IndexModel(IadvClientesINSSAppService advClientesINSSAppService)
    {
        _advClientesINSSAppService = advClientesINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesINSSGetListInput input)
    {
        var result = await _advClientesINSSAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesINSSAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesINSSFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:idInssAgendado")]
    public int? idInssAgendado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssAgendado")]
    public bool? inssAgendado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssData")]
    public string? inssData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssResultado")]
    public string? inssResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:tsInclusao")]
    public string? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:tsAlteracao")]
    public string? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssResultadoIndicadorOculto")]
    public bool? inssResultadoIndicadorOculto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:idStatus")]
    public int? idStatus { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesINSS:dataFinalizacao")]
    public string? dataFinalizacao { get; set; }
}
