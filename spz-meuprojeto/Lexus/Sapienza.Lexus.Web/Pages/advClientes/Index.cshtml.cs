using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advClientes;
using Sapienza.Lexus.advClientes.Dtos;

namespace Sapienza.Lexus.Web.Pages.advClientes;

public class IndexModel : Sapienza.LexusPageModel
{
    public advClientesFilterInput advClientesFilter { get; set; }
    
    private readonly IadvClientesAppService _advClientesAppService;

    public IndexModel(IadvClientesAppService advClientesAppService)
    {
        _advClientesAppService = advClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advClientesGetListInput input)
    {
        var result = await _advClientesAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advClientesAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advClientesFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idCliente")]
    public int? idCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:apelido")]
    public string? apelido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idGrupo")]
    public int? idGrupo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idSituacao")]
    public int? idSituacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:nome")]
    public string? nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:email")]
    public string? email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telCelular")]
    public string? telCelular { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telCelularObs")]
    public string? telCelularObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telFixo")]
    public string? telFixo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telFixoObs")]
    public string? telFixoObs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dataNascimento")]
    public string? dataNascimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:cpf")]
    public string? cpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:rg")]
    public string? rg { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:ctps")]
    public string? ctps { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:endereco")]
    public string? endereco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:complemento")]
    public string? complemento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:bairro")]
    public string? bairro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:cep")]
    public string? cep { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dataIngresso")]
    public string? dataIngresso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:observacoes")]
    public string? observacoes { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:naturalEstado")]
    public string? naturalEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:naturalCidade")]
    public string? naturalCidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:nomeDaMae")]
    public string? nomeDaMae { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dib")]
    public bool? dib { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dibData")]
    public string? dibData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dibIdTipoBeneficio")]
    public int? dibIdTipoBeneficio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idCargo")]
    public int? idCargo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telCelular2")]
    public string? telCelular2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telCelular2Obs")]
    public string? telCelular2Obs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telFixo2")]
    public string? telFixo2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:telFixo2Obs")]
    public string? telFixo2Obs { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:cnpj")]
    public string? cnpj { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:ie")]
    public string? ie { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idFornecedor")]
    public int? idFornecedor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssAgendado")]
    public bool? inssAgendado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssData")]
    public string? inssData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssResultado")]
    public string? inssResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:prospect")]
    public bool? prospect { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:whatsapp")]
    public bool? whatsapp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:pastaFTP")]
    public string? pastaFTP { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:responsavelPendencia")]
    public int? responsavelPendencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:comoChegou")]
    public int? comoChegou { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssTsInclusao")]
    public DateTime? inssTsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:foto")]
    public string? foto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:followBloqueadoAte")]
    public string? followBloqueadoAte { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:falecido")]
    public bool? falecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:senhaINSSDigital")]
    public string? senhaINSSDigital { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:idPrioridade")]
    public int? idPrioridade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:instagram")]
    public string? instagram { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:rgOrgaoExp")]
    public string? rgOrgaoExp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:nacionalidade")]
    public string? nacionalidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:estadocivil")]
    public string? estadocivil { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dcb")]
    public bool? dcb { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:dcbData")]
    public string? dcbData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:finIdUnidade")]
    public int? finIdUnidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advClientes:finIdCentroCusto")]
    public int? finIdCentroCusto { get; set; }
}
