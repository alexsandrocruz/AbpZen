using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosHonorarios.Dtos;

[Serializable]
public class advProcessosHonorariosDto : FullAuditedEntityDto<Guid>
{
    public int? idHonorario { get; set; }
    public int idProcesso { get; set; }
    public string dataPrevistaClienteReceber { get; set; }
    public string rpv { get; set; }
    public string precatorio { get; set; }
    public int? nrParcelasProcesso { get; set; }
    public double? valorHonorarios { get; set; }
    public string valorHonorariosTipo { get; set; }
    public string honorariosTextoFicha { get; set; }
    public double? valorHonorariosDestaque { get; set; }
    public string valorHonorariosDestaqueTipo { get; set; }
    public string dataPrevisaoHonorariosDestaque { get; set; }
    public double? imposto { get; set; }
    public double? complementoPositivo { get; set; }
    public double? sucumbencia { get; set; }
    public double? saldoDevedor { get; set; }
    public double? valorDeferido { get; set; }
    public string dataLiberacaoValorDeferido { get; set; }
    public int? idConta { get; set; }
    public string dataPrevisaoRepasseCliente { get; set; }
    public int? idContaPagar { get; set; }
    public string formaRecebimento { get; set; }
    public int? nrParcelasSomenteSucumbencia { get; set; }
    public double? sucumbenciaAdd { get; set; }
    public string sucumbenciaAddData { get; set; }
    public int? sucumbenciaAddIdBanco { get; set; }
    public bool? boleto { get; set; }
    public string emitir { get; set; }
    public bool? emitido { get; set; }
    public bool? nfComComplementoPositivo { get; set; }
    public string bancarioCpf { get; set; }
    public string herdeirosTipoValor { get; set; }
    public double? bancarioPerc { get; set; }
    public double? tarifa { get; set; }
    public string tarifaParcelas { get; set; }
    public string bancarioFavorecido { get; set; }
    public int? bancarioBancoId { get; set; }
    public string bancarioTipoConta { get; set; }
    public string bancarioAgencia { get; set; }
    public string bancarioConta { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string incluidoPor { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string alteradoPor { get; set; }
    public double? valorHonorariosDestaqueSomente { get; set; }
    public string valorHonorariosDestaqueTipoSomente { get; set; }
    public string dataPrevisaoHonorariosDestaqueSomente { get; set; }
    public int? idBancoDestaqueSomente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
