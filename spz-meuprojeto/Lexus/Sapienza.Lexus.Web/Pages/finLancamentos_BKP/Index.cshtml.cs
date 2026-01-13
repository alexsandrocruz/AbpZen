using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.finLancamentos_BKP;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;

namespace Sapienza.Lexus.Web.Pages.finLancamentos_BKP;

public class IndexModel : Sapienza.LexusPageModel
{
    public finLancamentos_BKPFilterInput finLancamentos_BKPFilter { get; set; }
    
    private readonly IfinLancamentos_BKPAppService _finLancamentos_BKPAppService;

    public IndexModel(IfinLancamentos_BKPAppService finLancamentos_BKPAppService)
    {
        _finLancamentos_BKPAppService = finLancamentos_BKPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(finLancamentos_BKPGetListInput input)
    {
        var result = await _finLancamentos_BKPAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _finLancamentos_BKPAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class finLancamentos_BKPFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idLancamento")]
    public int? idLancamento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:operacao")]
    public string? operacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idForma")]
    public int? idForma { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:modulo")]
    public string? modulo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idCadastro")]
    public int? idCadastro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idPedido")]
    public int? idPedido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:descricao")]
    public string? descricao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:nrDocumento")]
    public string? nrDocumento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:valor")]
    public double? valor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:dataEmissao")]
    public string? dataEmissao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:dataVencimento")]
    public string? dataVencimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:dataQuitacao")]
    public string? dataQuitacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:quitado")]
    public bool? quitado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:recorrente")]
    public bool? recorrente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:recorrenteChave")]
    public string? recorrenteChave { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:previsao")]
    public bool? previsao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:cobrancaEnviada")]
    public bool? cobrancaEnviada { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:parcelado")]
    public bool? parcelado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:identificacao")]
    public int? identificacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:observacao")]
    public string? observacao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:parcela")]
    public int? parcela { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:parcelaMaxima")]
    public int? parcelaMaxima { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:dataVencimentoOriginal")]
    public string? dataVencimentoOriginal { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:pagtoLiberado")]
    public int? pagtoLiberado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:dataParaPrevisao")]
    public string? dataParaPrevisao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:recorrenteVencendoVisto")]
    public bool? recorrenteVencendoVisto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:recebimentoFuturo")]
    public bool? recebimentoFuturo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:recebimentoFuturoRel")]
    public bool? recebimentoFuturoRel { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idTerceiro")]
    public int? idTerceiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:arquivoDocumento")]
    public string? arquivoDocumento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:arquivoComprovante")]
    public string? arquivoComprovante { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idClientePagar")]
    public int? idClientePagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idProcessoPagar")]
    public int? idProcessoPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:identificacaoPagar")]
    public string? identificacaoPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:identificacaoPagar2")]
    public string? identificacaoPagar2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:arquivoDocumento2")]
    public string? arquivoDocumento2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:arquivoComprovante2")]
    public string? arquivoComprovante2 { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:verba")]
    public bool? verba { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:verbaDataDe")]
    public string? verbaDataDe { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:verbaDataAte")]
    public string? verbaDataAte { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:verbaEstado")]
    public string? verbaEstado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:verbaCidade")]
    public string? verbaCidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idCentroResultado")]
    public int? idCentroResultado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:secundaria")]
    public bool? secundaria { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:geradoPeloProcesso")]
    public bool? geradoPeloProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:sequenciaHerdeiro")]
    public int? sequenciaHerdeiro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "finLancamentos_BKP:idUnidade")]
    public int? idUnidade { get; set; }
}
