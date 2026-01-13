using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advRevisaoDocumentos;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;

namespace Sapienza.Lexus.Web.Pages.advRevisaoDocumentos;

public class IndexModel : Sapienza.LexusPageModel
{
    public advRevisaoDocumentosFilterInput advRevisaoDocumentosFilter { get; set; }
    
    private readonly IadvRevisaoDocumentosAppService _advRevisaoDocumentosAppService;

    public IndexModel(IadvRevisaoDocumentosAppService advRevisaoDocumentosAppService)
    {
        _advRevisaoDocumentosAppService = advRevisaoDocumentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advRevisaoDocumentosGetListInput input)
    {
        var result = await _advRevisaoDocumentosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advRevisaoDocumentosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advRevisaoDocumentosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:idRevisao")]
    public int? idRevisao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:idUsuarioSolicitante")]
    public int? idUsuarioSolicitante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:idUsuarioRevisor")]
    public int? idUsuarioRevisor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:localRede")]
    public string? localRede { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:pendente")]
    public bool? pendente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:aprovado")]
    public bool? aprovado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:reprovado")]
    public bool? reprovado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:finalizado")]
    public bool? finalizado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:comentariosSolicitante")]
    public string? comentariosSolicitante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:comentariosRevisor")]
    public string? comentariosRevisor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advRevisaoDocumentos:ativo")]
    public bool? ativo { get; set; }
}
