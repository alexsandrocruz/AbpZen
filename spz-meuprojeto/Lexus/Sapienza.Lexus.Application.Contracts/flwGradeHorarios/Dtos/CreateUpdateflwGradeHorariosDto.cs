using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.flwGradeHorarios.Dtos;

[Serializable]
public class CreateUpdateflwGradeHorariosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idGrade { get; set; }
    [Required]
    public int idHistoricoTipo { get; set; }
    public int? manhaHorarioInicial { get; set; }
    public int? manhaIntervalo { get; set; }
    public int? manhaQtde { get; set; }
    public int? tardeHorarioInicial { get; set; }
    public int? tardeIntervalo { get; set; }
    public int? tardeQtde { get; set; }
    public bool? dom { get; set; }
    public bool? seg { get; set; }
    public bool? ter { get; set; }
    public bool? qua { get; set; }
    public bool? qui { get; set; }
    public bool? sex { get; set; }
    public bool? sab { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
