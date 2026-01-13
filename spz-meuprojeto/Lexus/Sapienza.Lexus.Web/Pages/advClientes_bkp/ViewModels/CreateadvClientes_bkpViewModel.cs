using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientes_bkp.ViewModels;

public class CreateadvClientes_bkpViewModel
{
    [Display(Name = "advClientes_bkp:idCliente")]
    public int? idCliente { get; set; }
    [Display(Name = "advClientes_bkp:apelido")]
    public string? apelido { get; set; }
    [Display(Name = "advClientes_bkp:idGrupo")]
    public int? idGrupo { get; set; }
    [Display(Name = "advClientes_bkp:idSituacao")]
    public int? idSituacao { get; set; }
    [Display(Name = "advClientes_bkp:nome")]
    public string? nome { get; set; }
    [Display(Name = "advClientes_bkp:email")]
    public string? email { get; set; }
    [Display(Name = "advClientes_bkp:telCelular")]
    public string? telCelular { get; set; }
    [Display(Name = "advClientes_bkp:telCelularObs")]
    public string? telCelularObs { get; set; }
    [Display(Name = "advClientes_bkp:telFixo")]
    public string? telFixo { get; set; }
    [Display(Name = "advClientes_bkp:telFixoObs")]
    public string? telFixoObs { get; set; }
    [Display(Name = "advClientes_bkp:dataNascimento")]
    public string? dataNascimento { get; set; }
    [Display(Name = "advClientes_bkp:cpf")]
    public string? cpf { get; set; }
    [Display(Name = "advClientes_bkp:rg")]
    public string? rg { get; set; }
    [Display(Name = "advClientes_bkp:ctps")]
    public string? ctps { get; set; }
    [Display(Name = "advClientes_bkp:endereco")]
    public string? endereco { get; set; }
    [Display(Name = "advClientes_bkp:numero")]
    public string? numero { get; set; }
    [Display(Name = "advClientes_bkp:complemento")]
    public string? complemento { get; set; }
    [Display(Name = "advClientes_bkp:bairro")]
    public string? bairro { get; set; }
    [Display(Name = "advClientes_bkp:cep")]
    public string? cep { get; set; }
    [Display(Name = "advClientes_bkp:estado")]
    public string? estado { get; set; }
    [Display(Name = "advClientes_bkp:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advClientes_bkp:dataIngresso")]
    public string? dataIngresso { get; set; }
    [Display(Name = "advClientes_bkp:observacoes")]
    public string? observacoes { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:ativo")]
    public bool ativo { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:tsInclusao")]
    public DateTime tsInclusao { get; set; }
    [Display(Name = "advClientes_bkp:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advClientes_bkp:naturalEstado")]
    public string? naturalEstado { get; set; }
    [Display(Name = "advClientes_bkp:naturalCidade")]
    public string? naturalCidade { get; set; }
    [Display(Name = "advClientes_bkp:nomeDaMae")]
    public string? nomeDaMae { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:dib")]
    public bool dib { get; set; }
    [Display(Name = "advClientes_bkp:dibData")]
    public string? dibData { get; set; }
    [Display(Name = "advClientes_bkp:dibIdTipoBeneficio")]
    public int? dibIdTipoBeneficio { get; set; }
    [Display(Name = "advClientes_bkp:idCargo")]
    public int? idCargo { get; set; }
    [Display(Name = "advClientes_bkp:telCelular2")]
    public string? telCelular2 { get; set; }
    [Display(Name = "advClientes_bkp:telCelular2Obs")]
    public string? telCelular2Obs { get; set; }
    [Display(Name = "advClientes_bkp:telFixo2")]
    public string? telFixo2 { get; set; }
    [Display(Name = "advClientes_bkp:telFixo2Obs")]
    public string? telFixo2Obs { get; set; }
    [Display(Name = "advClientes_bkp:cnpj")]
    public string? cnpj { get; set; }
    [Display(Name = "advClientes_bkp:ie")]
    public string? ie { get; set; }
    [Display(Name = "advClientes_bkp:idFornecedor")]
    public int? idFornecedor { get; set; }
    [Display(Name = "advClientes_bkp:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:inssAgendado")]
    public bool inssAgendado { get; set; }
    [Display(Name = "advClientes_bkp:inssData")]
    public string? inssData { get; set; }
    [Display(Name = "advClientes_bkp:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }
    [Display(Name = "advClientes_bkp:inssIdPosto")]
    public int? inssIdPosto { get; set; }
    [Display(Name = "advClientes_bkp:inssResultado")]
    public string? inssResultado { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:prospect")]
    public bool prospect { get; set; }
    [Display(Name = "advClientes_bkp:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [Display(Name = "advClientes_bkp:whatsapp")]
    public bool? whatsapp { get; set; }
    [Display(Name = "advClientes_bkp:pastaFTP")]
    public string? pastaFTP { get; set; }
    [Display(Name = "advClientes_bkp:inssResponsavel")]
    public int? inssResponsavel { get; set; }
    [Display(Name = "advClientes_bkp:responsavelPendencia")]
    public int? responsavelPendencia { get; set; }
    [Display(Name = "advClientes_bkp:comoChegou")]
    public int? comoChegou { get; set; }
    [Display(Name = "advClientes_bkp:inssProtocolo")]
    public string? inssProtocolo { get; set; }
    [Display(Name = "advClientes_bkp:inssTsInclusao")]
    public DateTime? inssTsInclusao { get; set; }
    [Display(Name = "advClientes_bkp:inssIdUsuarioInclusao")]
    public int? inssIdUsuarioInclusao { get; set; }
    [Display(Name = "advClientes_bkp:foto")]
    public string? foto { get; set; }
    [Display(Name = "advClientes_bkp:followBloqueadoAte")]
    public string? followBloqueadoAte { get; set; }
    [Required]
    [Display(Name = "advClientes_bkp:falecido")]
    public bool falecido { get; set; }
    [Display(Name = "advClientes_bkp:senhaINSSDigital")]
    public string? senhaINSSDigital { get; set; }
    [Display(Name = "advClientes_bkp:idPrioridade")]
    public int? idPrioridade { get; set; }
    [Display(Name = "advClientes_bkp:instagram")]
    public string? instagram { get; set; }
    [Display(Name = "advClientes_bkp:rgOrgaoExp")]
    public string? rgOrgaoExp { get; set; }
    [Display(Name = "advClientes_bkp:nacionalidade")]
    public string? nacionalidade { get; set; }
    [Display(Name = "advClientes_bkp:estadocivil")]
    public string? estadocivil { get; set; }
    [Display(Name = "advClientes_bkp:dcb")]
    public bool? dcb { get; set; }
    [Display(Name = "advClientes_bkp:dcbData")]
    public string? dcbData { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
