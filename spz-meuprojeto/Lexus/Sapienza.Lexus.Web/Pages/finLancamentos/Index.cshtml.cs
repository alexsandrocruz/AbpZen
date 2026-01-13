using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finLancamentos;
using Sapienza.Lexus.finLancamentos.Dtos;

namespace Sapienza.Lexus.Web.Pages.finLancamentos;

public class IndexModel : Sapienza.LexusPageModel
{
    public finLancamentosFilterInput finLancamentosFilter { get; set; }
    
    private readonly IfinLancamentosAppService _finLancamentosAppService;

    public IndexModel(IfinLancamentosAppService finLancamentosAppService)
    {
        _finLancamentosAppService = finLancamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finLancamentosGetListInput input)
    {
        var result = await _finLancamentosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finLancamentosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finLancamentosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:operacao")]
    public string? operacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idForma")]
    public int? idForma { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:modulo")]
    public string? modulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idCadastro")]
    public int? idCadastro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idPedido")]
    public int? idPedido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:nrDocumento")]
    public string? nrDocumento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:valor")]
    public double? valor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:dataEmissao")]
    public string? dataEmissao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:dataVencimento")]
    public string? dataVencimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:dataQuitacao")]
    public string? dataQuitacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:quitado")]
    public bool? quitado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:recorrente")]
    public bool? recorrente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:recorrenteChave")]
    public string? recorrenteChave { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:previsao")]
    public bool? previsao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:cobrancaEnviada")]
    public bool? cobrancaEnviada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:parcelado")]
    public bool? parcelado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:identificacao")]
    public int? identificacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:observacao")]
    public string? observacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:parcela")]
    public int? parcela { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:parcelaMaxima")]
    public int? parcelaMaxima { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:dataVencimentoOriginal")]
    public string? dataVencimentoOriginal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:pagtoLiberado")]
    public int? pagtoLiberado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:dataParaPrevisao")]
    public string? dataParaPrevisao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:recorrenteVencendoVisto")]
    public bool? recorrenteVencendoVisto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:recebimentoFuturo")]
    public bool? recebimentoFuturo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:recebimentoFuturoRel")]
    public bool? recebimentoFuturoRel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idTerceiro")]
    public int? idTerceiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:arquivoDocumento")]
    public string? arquivoDocumento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:arquivoComprovante")]
    public string? arquivoComprovante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idClientePagar")]
    public int? idClientePagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idProcessoPagar")]
    public int? idProcessoPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:identificacaoPagar")]
    public string? identificacaoPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:identificacaoPagar2")]
    public string? identificacaoPagar2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:arquivoDocumento2")]
    public string? arquivoDocumento2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:arquivoComprovante2")]
    public string? arquivoComprovante2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:verba")]
    public bool? verba { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:verbaDataDe")]
    public string? verbaDataDe { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:verbaDataAte")]
    public string? verbaDataAte { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:verbaEstado")]
    public string? verbaEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:verbaCidade")]
    public string? verbaCidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:secundaria")]
    public bool? secundaria { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:geradoPeloProcesso")]
    public bool? geradoPeloProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:sequenciaHerdeiro")]
    public int? sequenciaHerdeiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idUnidade")]
    public int? idUnidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:rateioFeito")]
    public bool? rateioFeito { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:naoAbatePagtoDoSaldoDoCliente")]
    public bool? naoAbatePagtoDoSaldoDoCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos:idHonorario")]
    public int? idHonorario { get; set; }
}
