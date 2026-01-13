using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advFornecedores.ViewModels;

public class EditadvFornecedoresViewModel
{
    [Display(Name = "advFornecedores:idFornecedor")]
    public int? idFornecedor { get; set; }
    [Display(Name = "advFornecedores:apelido")]
    public string? apelido { get; set; }
    [Display(Name = "advFornecedores:nome")]
    public string? nome { get; set; }
    [Display(Name = "advFornecedores:email")]
    public string? email { get; set; }
    [Display(Name = "advFornecedores:telCelular")]
    public string? telCelular { get; set; }
    [Display(Name = "advFornecedores:telCelularObs")]
    public string? telCelularObs { get; set; }
    [Display(Name = "advFornecedores:telFixo")]
    public string? telFixo { get; set; }
    [Display(Name = "advFornecedores:telFixoObs")]
    public string? telFixoObs { get; set; }
    [Display(Name = "advFornecedores:endereco")]
    public string? endereco { get; set; }
    [Display(Name = "advFornecedores:numero")]
    public string? numero { get; set; }
    [Display(Name = "advFornecedores:complemento")]
    public string? complemento { get; set; }
    [Display(Name = "advFornecedores:bairro")]
    public string? bairro { get; set; }
    [Display(Name = "advFornecedores:cep")]
    public string? cep { get; set; }
    [Display(Name = "advFornecedores:estado")]
    public string? estado { get; set; }
    [Display(Name = "advFornecedores:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advFornecedores:observacoes")]
    public string? observacoes { get; set; }
    [Display(Name = "advFornecedores:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advFornecedores:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advFornecedores:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advFornecedores:parceiroEmProcesso")]
    public bool? parceiroEmProcesso { get; set; }
    [Display(Name = "advFornecedores:parceiroEmProcessoPerc")]
    public double? parceiroEmProcessoPerc { get; set; }
    [Display(Name = "advFornecedores:idProfissional")]
    public int? idProfissional { get; set; }
    [Display(Name = "advFornecedores:foto")]
    public string? foto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
