using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.flwFollows;
using Sapienza.Lexus.flwFollows.Dtos;

namespace Sapienza.Lexus.Web.Pages.flwFollows;

public class IndexModel : Sapienza.LexusPageModel
{
    public flwFollowsFilterInput flwFollowsFilter { get; set; }
    
    private readonly IflwFollowsAppService _flwFollowsAppService;

    public IndexModel(IflwFollowsAppService flwFollowsAppService)
    {
        _flwFollowsAppService = flwFollowsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(flwFollowsGetListInput input)
    {
        var result = await _flwFollowsAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _flwFollowsAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class flwFollowsFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idFollow")]
    public int? idFollow { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idAcao")]
    public int? idAcao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:data")]
    public string? data { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:horario")]
    public int? horario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:comentario")]
    public string? comentario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:finalizado")]
    public bool? finalizado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:dataFinalizacao")]
    public string? dataFinalizacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:horarioFinalizacao")]
    public int? horarioFinalizacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:idOportunidade")]
    public int? idOportunidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:chegou")]
    public bool? chegou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:tsChegou")]
    public DateTime? tsChegou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:naoComparecimento")]
    public bool? naoComparecimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwFollows:prioridade")]
    public bool? prioridade { get; set; }
}
