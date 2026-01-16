// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientes;

/// <summary>
/// advClientes entity
/// </summary>
public class advClientes : FullAuditedAggregateRoot<Guid>
{
    public int? idCliente { get; set; }
    public string? apelido { get; set; }
    public int? idGrupo { get; set; }
    public int? idSituacao { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public string? telCelular { get; set; }
    public string? telCelularObs { get; set; }
    public string? telFixo { get; set; }
    public string? telFixoObs { get; set; }
    public string? dataNascimento { get; set; }
    public string? cpf { get; set; }
    public string? rg { get; set; }
    public string? ctps { get; set; }
    public string? endereco { get; set; }
    public string? numero { get; set; }
    public string? complemento { get; set; }
    public string? bairro { get; set; }
    public string? cep { get; set; }
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public string? dataIngresso { get; set; }
    public string? observacoes { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? naturalEstado { get; set; }
    public string? naturalCidade { get; set; }
    public string? nomeDaMae { get; set; }
    public bool? dib { get; set; }
    public string? dibData { get; set; }
    public int? dibIdTipoBeneficio { get; set; }
    public int? idCargo { get; set; }
    public string? telCelular2 { get; set; }
    public string? telCelular2Obs { get; set; }
    public string? telFixo2 { get; set; }
    public string? telFixo2Obs { get; set; }
    public string? cnpj { get; set; }
    public string? ie { get; set; }
    public int? idFornecedor { get; set; }
    public string? incluidoPor { get; set; }
    public bool? inssAgendado { get; set; }
    public string? inssData { get; set; }
    public int? inssIdTipoBeneficio { get; set; }
    public int? inssIdPosto { get; set; }
    public string? inssResultado { get; set; }
    public bool? prospect { get; set; }
    public int? idLocalAtendido { get; set; }
    public bool? whatsapp { get; set; }
    public string? pastaFTP { get; set; }
    public int? inssResponsavel { get; set; }
    public int? responsavelPendencia { get; set; }
    public int? comoChegou { get; set; }
    public string? inssProtocolo { get; set; }
    public DateTime? inssTsInclusao { get; set; }
    public int? inssIdUsuarioInclusao { get; set; }
    public string? foto { get; set; }
    public string? followBloqueadoAte { get; set; }
    public bool? falecido { get; set; }
    public string? senhaINSSDigital { get; set; }
    public int? idPrioridade { get; set; }
    public string? instagram { get; set; }
    public string? rgOrgaoExp { get; set; }
    public string? nacionalidade { get; set; }
    public string? estadocivil { get; set; }
    public bool? dcb { get; set; }
    public string? dcbData { get; set; }
    public int? finIdUnidade { get; set; }
    public int? finIdCentroCusto { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advClientesArquivosId { get; set; }
    public Guid? advClientesAtualizacoesId { get; set; }
    public Guid? advClientesChecklistId { get; set; }
    public Guid? advProcessosId { get; set; }
    public Guid advProcessosClientesId { get; set; }
    public Guid? advClientesHistoricosId { get; set; }
    public Guid? opoOportunidadesId { get; set; }
    public Guid? flwFollowsId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advClientesArquivos.advClientesArquivos? advClientesNav { get; set; }
    public virtual Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes? advClientesNav1 { get; set; }
    public virtual Sapienza.Lexus.advClientesChecklist.advClientesChecklist? advClientesNav2 { get; set; }
    public virtual Sapienza.Lexus.advProcessos.advProcessos? advClientesNav3 { get; set; }
    public virtual Sapienza.Lexus.advProcessosClientes.advProcessosClientes advClientesNav4 { get; set; }
    public virtual Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos? advClientesNav5 { get; set; }
    public virtual Sapienza.Lexus.opoOportunidades.opoOportunidades? advClientesNav6 { get; set; }
    public virtual Sapienza.Lexus.flwFollows.flwFollows? advClientesNav7 { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advCliCargos.advCliCargos> advClienteses { get; set; } = new List<Sapienza.Lexus.advCliCargos.advCliCargos>();
    public virtual ICollection<Sapienza.Lexus.advCliGrupos.advCliGrupos> advClientesesCollection { get; set; } = new List<Sapienza.Lexus.advCliGrupos.advCliGrupos>();
    public virtual ICollection<Sapienza.Lexus.advCliSituacoes.advCliSituacoes> advClientesesCollection1 { get; set; } = new List<Sapienza.Lexus.advCliSituacoes.advCliSituacoes>();
    public virtual ICollection<Sapienza.Lexus.advFornecedores.advFornecedores> advClientesesCollection2 { get; set; } = new List<Sapienza.Lexus.advFornecedores.advFornecedores>();
    public virtual ICollection<Sapienza.Lexus.advPostosINSS.advPostosINSS> advClientesesCollection3 { get; set; } = new List<Sapienza.Lexus.advPostosINSS.advPostosINSS>();

    protected advClientes()
    {
        // Required by EF Core
    }

    public advClientes(Guid id) : base(id)
    {
    }
}
