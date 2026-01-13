using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.fabHistoricoTipos;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;

namespace Sapienza.Lexus.Web.Pages.fabHistoricoTipos;

public class IndexModel : Sapienza.LexusPageModel
{
    public fabHistoricoTiposFilterInput fabHistoricoTiposFilter { get; set; }
    
    private readonly IfabHistoricoTiposAppService _fabHistoricoTiposAppService;

    public IndexModel(IfabHistoricoTiposAppService fabHistoricoTiposAppService)
    {
        _fabHistoricoTiposAppService = fabHistoricoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(fabHistoricoTiposGetListInput input)
    {
        var result = await _fabHistoricoTiposAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _fabHistoricoTiposAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class fabHistoricoTiposFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:idHistoricoTipo")]
    public int? idHistoricoTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:titulo")]
    public string? titulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "fabHistoricoTipos:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }
}
