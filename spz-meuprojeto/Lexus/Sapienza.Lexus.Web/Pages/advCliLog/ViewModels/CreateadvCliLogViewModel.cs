using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliLog.ViewModels;

public class CreateadvCliLogViewModel
{
    [Display(Name = "advCliLog:idLog")]
    public int? idLog { get; set; }
    [Required]
    [Display(Name = "advCliLog:idCliente")]
    public int idCliente { get; set; }
    [Required]
    [Display(Name = "advCliLog:idUsuario")]
    public int idUsuario { get; set; }
    [Required]
    [Display(Name = "advCliLog:acao")]
    public int acao { get; set; }
    [Required]
    [Display(Name = "advCliLog:idArea")]
    public int idArea { get; set; }
    [Display(Name = "advCliLog:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliLog:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advCliLog:idResponsavel")]
    public int? idResponsavel { get; set; }
    [Display(Name = "advCliLog:dataAgendamento")]
    public DateTime? dataAgendamento { get; set; }
    [Display(Name = "advCliLog:inssIdTipoBeneficio")]
    public int? inssIdTipoBeneficio { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
