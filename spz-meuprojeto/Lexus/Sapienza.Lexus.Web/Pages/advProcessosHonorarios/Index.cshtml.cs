using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.advProcessosHonorarios;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;

namespace Sapienza.Lexus.Web.Pages.advProcessosHonorarios;

public class IndexModel : Sapienza.LexusPageModel
{
    public advProcessosHonorariosFilterInput advProcessosHonorariosFilter { get; set; }
    
    private readonly IadvProcessosHonorariosAppService _advProcessosHonorariosAppService;

    public IndexModel(IadvProcessosHonorariosAppService advProcessosHonorariosAppService)
    {
        _advProcessosHonorariosAppService = advProcessosHonorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(advProcessosHonorariosGetListInput input)
    {
        var result = await _advProcessosHonorariosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _advProcessosHonorariosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class advProcessosHonorariosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:idHonorario")]
    public int? idHonorario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:idProcesso")]
    public int? idProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:dataPrevistaClienteReceber")]
    public string? dataPrevistaClienteReceber { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:rpv")]
    public string? rpv { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:precatorio")]
    public string? precatorio { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:nrParcelasProcesso")]
    public int? nrParcelasProcesso { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorarios")]
    public double? valorHonorarios { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorariosTipo")]
    public string? valorHonorariosTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:honorariosTextoFicha")]
    public string? honorariosTextoFicha { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaque")]
    public double? valorHonorariosDestaque { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueTipo")]
    public string? valorHonorariosDestaqueTipo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:dataPrevisaoHonorariosDestaque")]
    public string? dataPrevisaoHonorariosDestaque { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:imposto")]
    public double? imposto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:complementoPositivo")]
    public double? complementoPositivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:sucumbencia")]
    public double? sucumbencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:saldoDevedor")]
    public double? saldoDevedor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorDeferido")]
    public double? valorDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:dataLiberacaoValorDeferido")]
    public string? dataLiberacaoValorDeferido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:idConta")]
    public int? idConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:dataPrevisaoRepasseCliente")]
    public string? dataPrevisaoRepasseCliente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:idContaPagar")]
    public int? idContaPagar { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:formaRecebimento")]
    public string? formaRecebimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:nrParcelasSomenteSucumbencia")]
    public int? nrParcelasSomenteSucumbencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:sucumbenciaAdd")]
    public double? sucumbenciaAdd { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:sucumbenciaAddData")]
    public string? sucumbenciaAddData { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:sucumbenciaAddIdBanco")]
    public int? sucumbenciaAddIdBanco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:boleto")]
    public bool? boleto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:emitir")]
    public string? emitir { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:emitido")]
    public bool? emitido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:nfComComplementoPositivo")]
    public bool? nfComComplementoPositivo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:herdeirosTipoValor")]
    public string? herdeirosTipoValor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:tarifa")]
    public double? tarifa { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:tarifaParcelas")]
    public string? tarifaParcelas { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:bancarioConta")]
    public string? bancarioConta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:incluidoPor")]
    public string? incluidoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:alteradoPor")]
    public string? alteradoPor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueSomente")]
    public double? valorHonorariosDestaqueSomente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueTipoSomente")]
    public string? valorHonorariosDestaqueTipoSomente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:dataPrevisaoHonorariosDestaqueSomente")]
    public string? dataPrevisaoHonorariosDestaqueSomente { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "advProcessosHonorarios:idBancoDestaqueSomente")]
    public int? idBancoDestaqueSomente { get; set; }
}
