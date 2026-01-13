using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.opoOportunidades.ViewModels;

public class EditopoOportunidadesViewModel
{
    [Display(Name = "opoOportunidades:idOportunidade")]
    public int? idOportunidade { get; set; }
    [Required]
    [Display(Name = "opoOportunidades:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "opoOportunidades:idUsuario")]
    public int idUsuario { get; set; }
    [Required]
    [Display(Name = "opoOportunidades:idTipo")]
    public int idTipo { get; set; }
    [Required]
    [Display(Name = "opoOportunidades:idSituacao")]
    public int idSituacao { get; set; }
    [Display(Name = "opoOportunidades:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "opoOportunidades:numero")]
    public string? numero { get; set; }
    [Display(Name = "opoOportunidades:dataInicio")]
    public string? dataInicio { get; set; }
    [Display(Name = "opoOportunidades:dataEstimada")]
    public string? dataEstimada { get; set; }
    [Display(Name = "opoOportunidades:valorEstimado")]
    public double? valorEstimado { get; set; }
    [Display(Name = "opoOportunidades:comentario")]
    public string? comentario { get; set; }
    [Display(Name = "opoOportunidades:aproveitada")]
    public bool? aproveitada { get; set; }
    [Display(Name = "opoOportunidades:aproveitadaData")]
    public string? aproveitadaData { get; set; }
    [Display(Name = "opoOportunidades:cancelada")]
    public bool? cancelada { get; set; }
    [Display(Name = "opoOportunidades:canceladaMotivo")]
    public string? canceladaMotivo { get; set; }
    [Display(Name = "opoOportunidades:canceladaData")]
    public string? canceladaData { get; set; }
    [Display(Name = "opoOportunidades:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "opoOportunidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "opoOportunidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "opoOportunidades:indicadorCanceladoVisto")]
    public bool? indicadorCanceladoVisto { get; set; }
    [Display(Name = "opoOportunidades:valorEstimadoMensal")]
    public double? valorEstimadoMensal { get; set; }
    [Display(Name = "opoOportunidades:deProcesso")]
    public bool? deProcesso { get; set; }
    [Display(Name = "opoOportunidades:aproveitadaMotivo")]
    public string? aproveitadaMotivo { get; set; }
    [Display(Name = "opoOportunidades:numeroProcesso")]
    public int? numeroProcesso { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
