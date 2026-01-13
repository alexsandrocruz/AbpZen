using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesArquivos.ViewModels;

public class EditadvClientesArquivosViewModel
{
    [Display(Name = "advClientesArquivos:idArquivo")]
    public int? idArquivo { get; set; }
    [Required]
    [Display(Name = "advClientesArquivos:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "advClientesArquivos:idTipoArquivo")]
    public int idTipoArquivo { get; set; }
    [Display(Name = "advClientesArquivos:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "advClientesArquivos:arquivo")]
    public string? arquivo { get; set; }
    [Display(Name = "advClientesArquivos:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "advClientesArquivos:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "advClientesArquivos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advClientesArquivos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientesArquivos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advClientesArquivos:idProcesso")]
    public int? idProcesso { get; set; }
    [Display(Name = "advClientesArquivos:precisaRevisao")]
    public bool? precisaRevisao { get; set; }
    [Display(Name = "advClientesArquivos:idSolicitante")]
    public int? idSolicitante { get; set; }
    [Display(Name = "advClientesArquivos:solicitanteComentario")]
    public string? solicitanteComentario { get; set; }
    [Display(Name = "advClientesArquivos:idRevisor")]
    public int? idRevisor { get; set; }
    [Display(Name = "advClientesArquivos:revisorComentario")]
    public string? revisorComentario { get; set; }
    [Display(Name = "advClientesArquivos:reprovado")]
    public int? reprovado { get; set; }
    [Display(Name = "advClientesArquivos:pendenteVisualizacaoAprovacao")]
    public bool? pendenteVisualizacaoAprovacao { get; set; }
    [Display(Name = "advClientesArquivos:status")]
    public string? status { get; set; }
    [Display(Name = "advClientesArquivos:autoFTP")]
    public bool? autoFTP { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
