using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwGradeHorarios.Dtos;

[Serializable]
public class flwGradeHorariosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idGrade { get; set; }
    public int? idHistoricoTipo { get; set; }
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

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
