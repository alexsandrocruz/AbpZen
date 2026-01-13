using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.flwGradeHorarios;
using Sapienza.Lexus.flwGradeHorarios.Dtos;

namespace Sapienza.Lexus.Web.Pages.flwGradeHorarios;

public class IndexModel : Sapienza.LexusPageModel
{
    public flwGradeHorariosFilterInput flwGradeHorariosFilter { get; set; }
    
    private readonly IflwGradeHorariosAppService _flwGradeHorariosAppService;

    public IndexModel(IflwGradeHorariosAppService flwGradeHorariosAppService)
    {
        _flwGradeHorariosAppService = flwGradeHorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(flwGradeHorariosGetListInput input)
    {
        var result = await _flwGradeHorariosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _flwGradeHorariosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class flwGradeHorariosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:idGrade")]
    public int? idGrade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:idHistoricoTipo")]
    public int? idHistoricoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:manhaHorarioInicial")]
    public int? manhaHorarioInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:manhaIntervalo")]
    public int? manhaIntervalo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:manhaQtde")]
    public int? manhaQtde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:tardeHorarioInicial")]
    public int? tardeHorarioInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:tardeIntervalo")]
    public int? tardeIntervalo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:tardeQtde")]
    public int? tardeQtde { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:dom")]
    public bool? dom { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:seg")]
    public bool? seg { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:ter")]
    public bool? ter { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:qua")]
    public bool? qua { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:qui")]
    public bool? qui { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:sex")]
    public bool? sex { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "flwGradeHorarios:sab")]
    public bool? sab { get; set; }
}
