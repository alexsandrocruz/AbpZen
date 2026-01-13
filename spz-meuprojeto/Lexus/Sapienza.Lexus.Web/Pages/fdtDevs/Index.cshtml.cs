using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fdtDevs;
using Sapienza.Lexus.fdtDevs.Dtos;

namespace Sapienza.Lexus.Web.Pages.fdtDevs;

public class IndexModel : Sapienza.LexusPageModel
{
    public fdtDevsFilterInput fdtDevsFilter { get; set; }
    
    private readonly IfdtDevsAppService _fdtDevsAppService;

    public IndexModel(IfdtDevsAppService fdtDevsAppService)
    {
        _fdtDevsAppService = fdtDevsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fdtDevsGetListInput input)
    {
        var result = await _fdtDevsAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fdtDevsAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fdtDevsFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:idDev")]
    public int? idDev { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:pacote")]
    public string? pacote { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:pendente")]
    public bool? pendente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:aprovado")]
    public bool? aprovado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:reprovado")]
    public bool? reprovado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:finalizado")]
    public bool? finalizado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:comentariosRevisor")]
    public string? comentariosRevisor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fdtDevs:ativo")]
    public bool? ativo { get; set; }
}
