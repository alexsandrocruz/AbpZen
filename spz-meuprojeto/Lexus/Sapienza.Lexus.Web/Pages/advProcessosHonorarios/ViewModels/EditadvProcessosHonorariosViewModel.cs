using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessosHonorarios.ViewModels;

public class EditadvProcessosHonorariosViewModel
{
    [Display(Name = "advProcessosHonorarios:idHonorario")]
    public int? idHonorario { get; set; }
    [Required]
    [Display(Name = "advProcessosHonorarios:idProcesso")]
    public int idProcesso { get; set; }
    [Display(Name = "advProcessosHonorarios:dataPrevistaClienteReceber")]
    public string? dataPrevistaClienteReceber { get; set; }
    [Display(Name = "advProcessosHonorarios:rpv")]
    public string? rpv { get; set; }
    [Display(Name = "advProcessosHonorarios:precatorio")]
    public string? precatorio { get; set; }
    [Display(Name = "advProcessosHonorarios:nrParcelasProcesso")]
    public int? nrParcelasProcesso { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorarios")]
    public double? valorHonorarios { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorariosTipo")]
    public string? valorHonorariosTipo { get; set; }
    [Display(Name = "advProcessosHonorarios:honorariosTextoFicha")]
    public string? honorariosTextoFicha { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaque")]
    public double? valorHonorariosDestaque { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueTipo")]
    public string? valorHonorariosDestaqueTipo { get; set; }
    [Display(Name = "advProcessosHonorarios:dataPrevisaoHonorariosDestaque")]
    public string? dataPrevisaoHonorariosDestaque { get; set; }
    [Display(Name = "advProcessosHonorarios:imposto")]
    public double? imposto { get; set; }
    [Display(Name = "advProcessosHonorarios:complementoPositivo")]
    public double? complementoPositivo { get; set; }
    [Display(Name = "advProcessosHonorarios:sucumbencia")]
    public double? sucumbencia { get; set; }
    [Display(Name = "advProcessosHonorarios:saldoDevedor")]
    public double? saldoDevedor { get; set; }
    [Display(Name = "advProcessosHonorarios:valorDeferido")]
    public double? valorDeferido { get; set; }
    [Display(Name = "advProcessosHonorarios:dataLiberacaoValorDeferido")]
    public string? dataLiberacaoValorDeferido { get; set; }
    [Display(Name = "advProcessosHonorarios:idConta")]
    public int? idConta { get; set; }
    [Display(Name = "advProcessosHonorarios:dataPrevisaoRepasseCliente")]
    public string? dataPrevisaoRepasseCliente { get; set; }
    [Display(Name = "advProcessosHonorarios:idContaPagar")]
    public int? idContaPagar { get; set; }
    [Display(Name = "advProcessosHonorarios:formaRecebimento")]
    public string? formaRecebimento { get; set; }
    [Display(Name = "advProcessosHonorarios:nrParcelasSomenteSucumbencia")]
    public int? nrParcelasSomenteSucumbencia { get; set; }
    [Display(Name = "advProcessosHonorarios:sucumbenciaAdd")]
    public double? sucumbenciaAdd { get; set; }
    [Display(Name = "advProcessosHonorarios:sucumbenciaAddData")]
    public string? sucumbenciaAddData { get; set; }
    [Display(Name = "advProcessosHonorarios:sucumbenciaAddIdBanco")]
    public int? sucumbenciaAddIdBanco { get; set; }
    [Display(Name = "advProcessosHonorarios:boleto")]
    public bool? boleto { get; set; }
    [Display(Name = "advProcessosHonorarios:emitir")]
    public string? emitir { get; set; }
    [Display(Name = "advProcessosHonorarios:emitido")]
    public bool? emitido { get; set; }
    [Display(Name = "advProcessosHonorarios:nfComComplementoPositivo")]
    public bool? nfComComplementoPositivo { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioCpf")]
    public string? bancarioCpf { get; set; }
    [Display(Name = "advProcessosHonorarios:herdeirosTipoValor")]
    public string? herdeirosTipoValor { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioPerc")]
    public double? bancarioPerc { get; set; }
    [Display(Name = "advProcessosHonorarios:tarifa")]
    public double? tarifa { get; set; }
    [Display(Name = "advProcessosHonorarios:tarifaParcelas")]
    public string? tarifaParcelas { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioFavorecido")]
    public string? bancarioFavorecido { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioBancoId")]
    public int? bancarioBancoId { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioTipoConta")]
    public string? bancarioTipoConta { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioAgencia")]
    public string? bancarioAgencia { get; set; }
    [Display(Name = "advProcessosHonorarios:bancarioConta")]
    public string? bancarioConta { get; set; }
    [Display(Name = "advProcessosHonorarios:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProcessosHonorarios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProcessosHonorarios:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advProcessosHonorarios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advProcessosHonorarios:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueSomente")]
    public double? valorHonorariosDestaqueSomente { get; set; }
    [Display(Name = "advProcessosHonorarios:valorHonorariosDestaqueTipoSomente")]
    public string? valorHonorariosDestaqueTipoSomente { get; set; }
    [Display(Name = "advProcessosHonorarios:dataPrevisaoHonorariosDestaqueSomente")]
    public string? dataPrevisaoHonorariosDestaqueSomente { get; set; }
    [Display(Name = "advProcessosHonorarios:idBancoDestaqueSomente")]
    public int? idBancoDestaqueSomente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
