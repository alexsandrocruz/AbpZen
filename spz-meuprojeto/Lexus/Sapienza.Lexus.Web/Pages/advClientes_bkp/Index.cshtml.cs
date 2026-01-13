using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientes_bkp;
using Sapienza.Lexus.advClientes_bkp.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientes_bkp;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientes_bkpFilterInput advClientes_bkpFilter { get; set; }
    
    private readonly IadvClientes_bkpAppService _advClientes_bkpAppService;

    public IndexModel(IadvClientes_bkpAppService advClientes_bkpAppService)
    {
        _advClientes_bkpAppService = advClientes_bkpAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientes_bkpGetListInput input)
    {
        var result = await _advClientes_bkpAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientes_bkpAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientes_bkpFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:apelido")]
    public string? apelido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idSituacao")]
    public int? idSituacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:nome")]
    public string? nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:email")]
    public string? email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telCelular")]
    public string? telCelular { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telCelularObs")]
    public string? telCelularObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telFixo")]
    public string? telFixo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telFixoObs")]
    public string? telFixoObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dataNascimento")]
    public string? dataNascimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:cpf")]
    public string? cpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:rg")]
    public string? rg { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:ctps")]
    public string? ctps { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:endereco")]
    public string? endereco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:complemento")]
    public string? complemento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:bairro")]
    public string? bairro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:cep")]
    public string? cep { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dataIngresso")]
    public string? dataIngresso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:observacoes")]
    public string? observacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:naturalEstado")]
    public string? naturalEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:naturalCidade")]
    public string? naturalCidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:nomeDaMae")]
    public string? nomeDaMae { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dib")]
    public bool? dib { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dibData")]
    public string? dibData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dibIdTipoBeneficio")]
    public int? dibIdTipoBeneficio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idCargo")]
    public int? idCargo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telCelular2")]
    public string? telCelular2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telCelular2Obs")]
    public string? telCelular2Obs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telFixo2")]
    public string? telFixo2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:telFixo2Obs")]
    public string? telFixo2Obs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:cnpj")]
    public string? cnpj { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:ie")]
    public string? ie { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idFornecedor")]
    public int? idFornecedor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssAgendado")]
    public bool? inssAgendado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssData")]
    public string? inssData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssResultado")]
    public string? inssResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:prospect")]
    public bool? prospect { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:whatsapp")]
    public bool? whatsapp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:pastaFTP")]
    public string? pastaFTP { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:responsavelPendencia")]
    public int? responsavelPendencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:comoChegou")]
    public int? comoChegou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssTsInclusao")]
    public DateTime? inssTsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:foto")]
    public string? foto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:followBloqueadoAte")]
    public string? followBloqueadoAte { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:falecido")]
    public bool? falecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:senhaINSSDigital")]
    public string? senhaINSSDigital { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:idPrioridade")]
    public int? idPrioridade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:instagram")]
    public string? instagram { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:rgOrgaoExp")]
    public string? rgOrgaoExp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:nacionalidade")]
    public string? nacionalidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:estadocivil")]
    public string? estadocivil { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dcb")]
    public bool? dcb { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes_bkp:dcbData")]
    public string? dcbData { get; set; }
}
