using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advVerbas.ViewModels;

public class CreateadvVerbasViewModel
{
    [Display(Name = "advVerbas:idVerba")]
    public int? idVerba { get; set; }
    [Required]
    [Display(Name = "advVerbas:idTipo")]
    public int idTipo { get; set; }
    [Required]
    [Display(Name = "advVerbas:idProfissional")]
    public int idProfissional { get; set; }
    [Display(Name = "advVerbas:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advVerbas:idLancamento")]
    public int? idLancamento { get; set; }
    [Display(Name = "advVerbas:valor")]
    public double? valor { get; set; }
    [Required]
    [Display(Name = "advVerbas:dataDe")]
    public string dataDe { get; set; } = string.Empty;
    [Required]
    [Display(Name = "advVerbas:dataAte")]
    public string dataAte { get; set; } = string.Empty;
    [Display(Name = "advVerbas:estado")]
    public string? estado { get; set; }
    [Display(Name = "advVerbas:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advVerbas:comprovante")]
    public bool? comprovante { get; set; }
    [Display(Name = "advVerbas:comprovanteArquivo")]
    public string? comprovanteArquivo { get; set; }
    [Display(Name = "advVerbas:solicitacao")]
    public bool? solicitacao { get; set; }
    [Display(Name = "advVerbas:aceito")]
    public bool? aceito { get; set; }
    [Display(Name = "advVerbas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advVerbas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advVerbas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advVerbas:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advVerbas:alteradoPor")]
    public string? alteradoPor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
