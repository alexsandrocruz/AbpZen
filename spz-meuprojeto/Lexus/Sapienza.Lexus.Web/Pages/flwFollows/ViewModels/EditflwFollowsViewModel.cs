using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.flwFollows.ViewModels;

public class EditflwFollowsViewModel
{
    [Display(Name = "flwFollows:idFollow")]
    public int? idFollow { get; set; }
    [Required]
    [Display(Name = "flwFollows:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "flwFollows:idAcao")]
    public int idAcao { get; set; }
    [Required]
    [Display(Name = "flwFollows:idTipo")]
    public int idTipo { get; set; }
    [Required]
    [Display(Name = "flwFollows:idUsuario")]
    public int idUsuario { get; set; }
    [Required]
    [Display(Name = "flwFollows:data")]
    public string data { get; set; } = string.Empty;
    [Display(Name = "flwFollows:horario")]
    public int? horario { get; set; }
    [Display(Name = "flwFollows:comentario")]
    public string? comentario { get; set; }
    [Display(Name = "flwFollows:finalizado")]
    public bool? finalizado { get; set; }
    [Display(Name = "flwFollows:dataFinalizacao")]
    public string? dataFinalizacao { get; set; }
    [Display(Name = "flwFollows:horarioFinalizacao")]
    public int? horarioFinalizacao { get; set; }
    [Display(Name = "flwFollows:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "flwFollows:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "flwFollows:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "flwFollows:idOportunidade")]
    public int? idOportunidade { get; set; }
    [Display(Name = "flwFollows:chegou")]
    public bool? chegou { get; set; }
    [Display(Name = "flwFollows:tsChegou")]
    public DateTime? tsChegou { get; set; }
    [Display(Name = "flwFollows:naoComparecimento")]
    public bool? naoComparecimento { get; set; }
    [Display(Name = "flwFollows:prioridade")]
    public bool? prioridade { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
