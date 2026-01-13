using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuUsuarios.ViewModels;

public class EditusuUsuariosViewModel
{
    [Display(Name = "usuUsuarios:idUsuario")]
    public int? idUsuario { get; set; }
    [Display(Name = "usuUsuarios:nome")]
    public string? nome { get; set; }
    [Display(Name = "usuUsuarios:sobrenome")]
    public string? sobrenome { get; set; }
    [Required]
    [Display(Name = "usuUsuarios:idArea")]
    public int idArea { get; set; }
    [Required]
    [Display(Name = "usuUsuarios:idCargo")]
    public int idCargo { get; set; }
    [Display(Name = "usuUsuarios:login")]
    public string? login { get; set; }
    [Display(Name = "usuUsuarios:senha")]
    public string? senha { get; set; }
    [Display(Name = "usuUsuarios:diaNascimento")]
    public int? diaNascimento { get; set; }
    [Display(Name = "usuUsuarios:mesNascimento")]
    public int? mesNascimento { get; set; }
    [Display(Name = "usuUsuarios:anoNascimento")]
    public int? anoNascimento { get; set; }
    [Display(Name = "usuUsuarios:email")]
    public string? email { get; set; }
    [Display(Name = "usuUsuarios:telCelular")]
    public string? telCelular { get; set; }
    [Display(Name = "usuUsuarios:telFixo")]
    public string? telFixo { get; set; }
    [Display(Name = "usuUsuarios:endereco")]
    public string? endereco { get; set; }
    [Display(Name = "usuUsuarios:numero")]
    public string? numero { get; set; }
    [Display(Name = "usuUsuarios:complemento")]
    public string? complemento { get; set; }
    [Display(Name = "usuUsuarios:bairro")]
    public string? bairro { get; set; }
    [Display(Name = "usuUsuarios:cep")]
    public string? cep { get; set; }
    [Display(Name = "usuUsuarios:estado")]
    public string? estado { get; set; }
    [Display(Name = "usuUsuarios:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "usuUsuarios:cpf")]
    public string? cpf { get; set; }
    [Display(Name = "usuUsuarios:banco")]
    public string? banco { get; set; }
    [Display(Name = "usuUsuarios:agencia")]
    public string? agencia { get; set; }
    [Display(Name = "usuUsuarios:conta")]
    public string? conta { get; set; }
    [Display(Name = "usuUsuarios:foto")]
    public string? foto { get; set; }
    [Display(Name = "usuUsuarios:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "usuUsuarios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "usuUsuarios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "usuUsuarios:cor")]
    public string? cor { get; set; }
    [Display(Name = "usuUsuarios:dashboardInicial")]
    public string? dashboardInicial { get; set; }
    [Display(Name = "usuUsuarios:tokenPhoneApp")]
    public string? tokenPhoneApp { get; set; }
    [Display(Name = "usuUsuarios:estadoCivil")]
    public string? estadoCivil { get; set; }
    [Display(Name = "usuUsuarios:nrFilhos")]
    public int? nrFilhos { get; set; }
    [Display(Name = "usuUsuarios:idadeFilhoMenor")]
    public int? idadeFilhoMenor { get; set; }
    [Display(Name = "usuUsuarios:formacaoAcademica")]
    public string? formacaoAcademica { get; set; }
    [Display(Name = "usuUsuarios:regiao")]
    public string? regiao { get; set; }
    [Display(Name = "usuUsuarios:idSuperior")]
    public int? idSuperior { get; set; }
    [Display(Name = "usuUsuarios:master")]
    public bool? master { get; set; }
    [Display(Name = "usuUsuarios:mediaConsumoLitro")]
    public int? mediaConsumoLitro { get; set; }
    [Display(Name = "usuUsuarios:distanciasIguais")]
    public string? distanciasIguais { get; set; }
    [Display(Name = "usuUsuarios:chaveChamados")]
    public string? chaveChamados { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
