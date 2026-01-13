using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientesArquivos;
using Sapienza.Lexus.advClientesArquivos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientesArquivos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesArquivosFilterInput advClientesArquivosFilter { get; set; }
    
    private readonly IadvClientesArquivosAppService _advClientesArquivosAppService;

    public IndexModel(IadvClientesArquivosAppService advClientesArquivosAppService)
    {
        _advClientesArquivosAppService = advClientesArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesArquivosGetListInput input)
    {
        var result = await _advClientesArquivosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesArquivosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesArquivosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idArquivo")]
    public int? idArquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idTipoArquivo")]
    public int? idTipoArquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:arquivo")]
    public string? arquivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:precisaRevisao")]
    public bool? precisaRevisao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idSolicitante")]
    public int? idSolicitante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:solicitanteComentario")]
    public string? solicitanteComentario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:idRevisor")]
    public int? idRevisor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:revisorComentario")]
    public string? revisorComentario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:reprovado")]
    public int? reprovado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:pendenteVisualizacaoAprovacao")]
    public bool? pendenteVisualizacaoAprovacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:status")]
    public string? status { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientesArquivos:autoFTP")]
    public bool? autoFTP { get; set; }
}
