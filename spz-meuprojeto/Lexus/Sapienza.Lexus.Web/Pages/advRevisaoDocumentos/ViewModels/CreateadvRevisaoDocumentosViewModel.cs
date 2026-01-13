using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advRevisaoDocumentos.ViewModels;

public class CreateadvRevisaoDocumentosViewModel
{
    [Display(Name = "advRevisaoDocumentos:idRevisao")]
    public int? idRevisao { get; set; }
    [Required]
    [Display(Name = "advRevisaoDocumentos:idUsuarioSolicitante")]
    public int idUsuarioSolicitante { get; set; }
    [Required]
    [Display(Name = "advRevisaoDocumentos:idUsuarioRevisor")]
    public int idUsuarioRevisor { get; set; }
    [Display(Name = "advRevisaoDocumentos:localRede")]
    public string? localRede { get; set; }
    [Display(Name = "advRevisaoDocumentos:pendente")]
    public bool? pendente { get; set; }
    [Display(Name = "advRevisaoDocumentos:aprovado")]
    public bool? aprovado { get; set; }
    [Display(Name = "advRevisaoDocumentos:reprovado")]
    public bool? reprovado { get; set; }
    [Display(Name = "advRevisaoDocumentos:finalizado")]
    public bool? finalizado { get; set; }
    [Display(Name = "advRevisaoDocumentos:comentariosSolicitante")]
    public string? comentariosSolicitante { get; set; }
    [Display(Name = "advRevisaoDocumentos:comentariosRevisor")]
    public string? comentariosRevisor { get; set; }
    [Display(Name = "advRevisaoDocumentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advRevisaoDocumentos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advRevisaoDocumentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advRevisaoDocumentos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "advRevisaoDocumentos:ativo")]
    public bool? ativo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
