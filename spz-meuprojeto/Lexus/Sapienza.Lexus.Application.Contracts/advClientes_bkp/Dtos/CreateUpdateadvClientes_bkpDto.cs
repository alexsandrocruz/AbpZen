using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.advClientes_bkp.Dtos;

[Serializable]
public class CreateUpdateadvClientes_bkpDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idCliente { get; set; }
    public string apelido { get; set; }
    public int? idGrupo { get; set; }
    public int? idSituacao { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string telCelular { get; set; }
    public string telCelularObs { get; set; }
    public string telFixo { get; set; }
    public string telFixoObs { get; set; }
    public string dataNascimento { get; set; }
    public string cpf { get; set; }
    public string rg { get; set; }
    public string ctps { get; set; }
    public string endereco { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string cep { get; set; }
    public string estado { get; set; }
    public string cidade { get; set; }
    public string dataIngresso { get; set; }
    public string observacoes { get; set; }
    [Required]
    public bool ativo { get; set; }
    [Required]
    public DateTime tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string naturalEstado { get; set; }
    public string naturalCidade { get; set; }
    public string nomeDaMae { get; set; }
    [Required]
    public bool dib { get; set; }
    public string dibData { get; set; }
    public int? dibIdTipoBeneficio { get; set; }
    public int? idCargo { get; set; }
    public string telCelular2 { get; set; }
    public string telCelular2Obs { get; set; }
    public string telFixo2 { get; set; }
    public string telFixo2Obs { get; set; }
    public string cnpj { get; set; }
    public string ie { get; set; }
    public int? idFornecedor { get; set; }
    public string incluidoPor { get; set; }
    [Required]
    public bool inssAgendado { get; set; }
    public string inssData { get; set; }
    public int? inssIdTipoBeneficio { get; set; }
    public int? inssIdPosto { get; set; }
    public string inssResultado { get; set; }
    [Required]
    public bool prospect { get; set; }
    public int? idLocalAtendido { get; set; }
    public bool? whatsapp { get; set; }
    public string pastaFTP { get; set; }
    public int? inssResponsavel { get; set; }
    public int? responsavelPendencia { get; set; }
    public int? comoChegou { get; set; }
    public string inssProtocolo { get; set; }
    public DateTime? inssTsInclusao { get; set; }
    public int? inssIdUsuarioInclusao { get; set; }
    public string foto { get; set; }
    public string followBloqueadoAte { get; set; }
    [Required]
    public bool falecido { get; set; }
    public string senhaINSSDigital { get; set; }
    public int? idPrioridade { get; set; }
    public string instagram { get; set; }
    public string rgOrgaoExp { get; set; }
    public string nacionalidade { get; set; }
    public string estadocivil { get; set; }
    public bool? dcb { get; set; }
    public string dcbData { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
