using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finPlanoContas.ViewModels;

public class EditfinPlanoContasViewModel
{
    [Display(Name = "finPlanoContas:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [Required]
    [Display(Name = "finPlanoContas:idGrupo")]
    public int idGrupo { get; set; }
    [Display(Name = "finPlanoContas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finPlanoContas:codigo")]
    public string? codigo { get; set; }
    [Display(Name = "finPlanoContas:pagamentoSempreLiberado")]
    public bool? pagamentoSempreLiberado { get; set; }
    [Display(Name = "finPlanoContas:permiteLancamentoQuitado")]
    public bool? permiteLancamentoQuitado { get; set; }
    [Display(Name = "finPlanoContas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finPlanoContas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finPlanoContas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finPlanoContas:padraoVendas")]
    public bool? padraoVendas { get; set; }
    [Display(Name = "finPlanoContas:antecipaVencimento")]
    public bool? antecipaVencimento { get; set; }
    [Display(Name = "finPlanoContas:terceiroNivel")]
    public bool? terceiroNivel { get; set; }
    [Display(Name = "finPlanoContas:criarPeloFinanceiro")]
    public bool? criarPeloFinanceiro { get; set; }
    [Display(Name = "finPlanoContas:valoresRestritos")]
    public bool? valoresRestritos { get; set; }
    [Display(Name = "finPlanoContas:naoAbatePagtoDoSaldoDoCliente")]
    public bool? naoAbatePagtoDoSaldoDoCliente { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
