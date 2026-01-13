using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.flwGradeHorarios.ViewModels;

public class EditflwGradeHorariosViewModel
{
    [Display(Name = "flwGradeHorarios:idGrade")]
    public int? idGrade { get; set; }
    [Required]
    [Display(Name = "flwGradeHorarios:idHistoricoTipo")]
    public int idHistoricoTipo { get; set; }
    [Display(Name = "flwGradeHorarios:manhaHorarioInicial")]
    public int? manhaHorarioInicial { get; set; }
    [Display(Name = "flwGradeHorarios:manhaIntervalo")]
    public int? manhaIntervalo { get; set; }
    [Display(Name = "flwGradeHorarios:manhaQtde")]
    public int? manhaQtde { get; set; }
    [Display(Name = "flwGradeHorarios:tardeHorarioInicial")]
    public int? tardeHorarioInicial { get; set; }
    [Display(Name = "flwGradeHorarios:tardeIntervalo")]
    public int? tardeIntervalo { get; set; }
    [Display(Name = "flwGradeHorarios:tardeQtde")]
    public int? tardeQtde { get; set; }
    [Display(Name = "flwGradeHorarios:dom")]
    public bool? dom { get; set; }
    [Display(Name = "flwGradeHorarios:seg")]
    public bool? seg { get; set; }
    [Display(Name = "flwGradeHorarios:ter")]
    public bool? ter { get; set; }
    [Display(Name = "flwGradeHorarios:qua")]
    public bool? qua { get; set; }
    [Display(Name = "flwGradeHorarios:qui")]
    public bool? qui { get; set; }
    [Display(Name = "flwGradeHorarios:sex")]
    public bool? sex { get; set; }
    [Display(Name = "flwGradeHorarios:sab")]
    public bool? sab { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
