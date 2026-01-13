using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.opoOportunidades;
using Sapienza.Lexus.opoOportunidades.Dtos;

namespace Sapienza.Lexus.Web.Pages.opoOportunidades;

public class IndexModel : Sapienza.LexusPageModel
{
    public opoOportunidadesFilterInput opoOportunidadesFilter { get; set; }
    
    private readonly IopoOportunidadesAppService _opoOportunidadesAppService;

    public IndexModel(IopoOportunidadesAppService opoOportunidadesAppService)
    {
        _opoOportunidadesAppService = opoOportunidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(opoOportunidadesGetListInput input)
    {
        var result = await _opoOportunidadesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _opoOportunidadesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class opoOportunidadesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:idOportunidade")]
    public int? idOportunidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:idTipo")]
    public int? idTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:idSituacao")]
    public int? idSituacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:dataInicio")]
    public string? dataInicio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:dataEstimada")]
    public string? dataEstimada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:valorEstimado")]
    public double? valorEstimado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:comentario")]
    public string? comentario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:aproveitada")]
    public bool? aproveitada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:aproveitadaData")]
    public string? aproveitadaData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:cancelada")]
    public bool? cancelada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:canceladaMotivo")]
    public string? canceladaMotivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:canceladaData")]
    public string? canceladaData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:indicadorCanceladoVisto")]
    public bool? indicadorCanceladoVisto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:valorEstimadoMensal")]
    public double? valorEstimadoMensal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:deProcesso")]
    public bool? deProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:aproveitadaMotivo")]
    public string? aproveitadaMotivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "opoOportunidades:numeroProcesso")]
    public int? numeroProcesso { get; set; }
}
