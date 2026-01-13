using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientes.ViewModels;

public class CreateadvClientesViewModel
{
    [Display(Name = "advClientes:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "advClientes:apelido")]
    public string? apelido { get; set; }
    [Display(Name = "advClientes:idGrupo")]
    public int? idGrupo { get; set; }
    [Display(Name = "advClientes:idSituacao")]
    public int? idSituacao { get; set; }
    [Display(Name = "advClientes:nome")]
    public string? nome { get; set; }
    [Display(Name = "advClientes:email")]
    public string? email { get; set; }
    [Display(Name = "advClientes:telCelular")]
    public string? telCelular { get; set; }
    [Display(Name = "advClientes:telCelularObs")]
    public string? telCelularObs { get; set; }
    [Display(Name = "advClientes:telFixo")]
    public string? telFixo { get; set; }
    [Display(Name = "advClientes:telFixoObs")]
    public string? telFixoObs { get; set; }
    [Display(Name = "advClientes:dataNascimento")]
    public string? dataNascimento { get; set; }
    [Display(Name = "advClientes:cpf")]
    public string? cpf { get; set; }
    [Display(Name = "advClientes:rg")]
    public string? rg { get; set; }
    [Display(Name = "advClientes:ctps")]
    public string? ctps { get; set; }
    [Display(Name = "advClientes:endereco")]
    public string? endereco { get; set; }
    [Display(Name = "advClientes:numero")]
    public string? numero { get; set; }
    [Display(Name = "advClientes:complemento")]
    public string? complemento { get; set; }
    [Display(Name = "advClientes:bairro")]
    public string? bairro { get; set; }
    [Display(Name = "advClientes:cep")]
    public string? cep { get; set; }
    [Display(Name = "advClientes:estado")]
    public string? estado { get; set; }
    [Display(Name = "advClientes:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advClientes:dataIngresso")]
    public string? dataIngresso { get; set; }
    [Display(Name = "advClientes:observacoes")]
    public string? observacoes { get; set; }
    [Display(Name = "advClientes:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advClientes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advClientes:naturalEstado")]
    public string? naturalEstado { get; set; }
    [Display(Name = "advClientes:naturalCidade")]
    public string? naturalCidade { get; set; }
    [Display(Name = "advClientes:nomeDaMae")]
    public string? nomeDaMae { get; set; }
    [Display(Name = "advClientes:dib")]
    public bool? dib { get; set; }
    [Display(Name = "advClientes:dibData")]
    public string? dibData { get; set; }
    [Display(Name = "advClientes:dibIdTipoBeneficio")]
    public int? dibIdTipoBeneficio { get; set; }
    [Display(Name = "advClientes:idCargo")]
    public int? idCargo { get; set; }
    [Display(Name = "advClientes:telCelular2")]
    public string? telCelular2 { get; set; }
    [Display(Name = "advClientes:telCelular2Obs")]
    public string? telCelular2Obs { get; set; }
    [Display(Name = "advClientes:telFixo2")]
    public string? telFixo2 { get; set; }
    [Display(Name = "advClientes:telFixo2Obs")]
    public string? telFixo2Obs { get; set; }
    [Display(Name = "advClientes:cnpj")]
    public string? cnpj { get; set; }
    [Display(Name = "advClientes:ie")]
    public string? ie { get; set; }
    [Display(Name = "advClientes:idFornecedor")]
    public int? idFornecedor { get; set; }
    [Display(Name = "advClientes:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advClientes:inssAgendado")]
    public bool? inssAgendado { get; set; }
    [Display(Name = "advClientes:inssData")]
    public string? inssData { get; set; }
    [Display(Name = "advClientes:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [Display(Name = "advClientes:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [Display(Name = "advClientes:inssResultado")]
    public string? inssResultado { get; set; }
    [Display(Name = "advClientes:prospect")]
    public bool? prospect { get; set; }
    [Display(Name = "advClientes:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [Display(Name = "advClientes:whatsapp")]
    public bool? whatsapp { get; set; }
    [Display(Name = "advClientes:pastaFTP")]
    public string? pastaFTP { get; set; }
    [Display(Name = "advClientes:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [Display(Name = "advClientes:responsavelPendencia")]
    public int? responsavelPendencia { get; set; }
    [Display(Name = "advClientes:comoChegou")]
    public int? comoChegou { get; set; }
    [Display(Name = "advClientes:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [Display(Name = "advClientes:inssTsInclusao")]
    public DateTime? inssTsInclusao { get; set; }
    [Display(Name = "advClientes:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [Display(Name = "advClientes:foto")]
    public string? foto { get; set; }
    [Display(Name = "advClientes:followBloqueadoAte")]
    public string? followBloqueadoAte { get; set; }
    [Display(Name = "advClientes:falecido")]
    public bool? falecido { get; set; }
    [Display(Name = "advClientes:senhaINSSDigital")]
    public string? senhaINSSDigital { get; set; }
    [Display(Name = "advClientes:idPrioridade")]
    public int? idPrioridade { get; set; }
    [Display(Name = "advClientes:instagram")]
    public string? instagram { get; set; }
    [Display(Name = "advClientes:rgOrgaoExp")]
    public string? rgOrgaoExp { get; set; }
    [Display(Name = "advClientes:nacionalidade")]
    public string? nacionalidade { get; set; }
    [Display(Name = "advClientes:estadocivil")]
    public string? estadocivil { get; set; }
    [Display(Name = "advClientes:dcb")]
    public bool? dcb { get; set; }
    [Display(Name = "advClientes:dcbData")]
    public string? dcbData { get; set; }
    [Display(Name = "advClientes:finIdUnidade")]
    public int? finIdUnidade { get; set; }
    [Display(Name = "advClientes:finIdCentroCusto")]
    public int? finIdCentroCusto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
