using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPreProcessosCheckLists.Dtos;

[Serializable]
public class advPreProcessosCheckListsGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPreCheckList { get; set; }
    public int? idProcesso { get; set; }
    public int? idGrupo { get; set; }
    public int? idCheckList { get; set; }
    public DateTime? tsInclusao { get; set; }
    public string? grupo { get; set; }
    public string? item { get; set; }
    public bool? concluido { get; set; }
    public string? tsConclusao { get; set; }
    public int? idResponsavel { get; set; }
    public int? ordem { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
