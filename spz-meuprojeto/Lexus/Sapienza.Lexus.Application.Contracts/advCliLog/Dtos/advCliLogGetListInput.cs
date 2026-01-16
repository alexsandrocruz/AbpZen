using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliLog.Dtos;

[Serializable]
public class advCliLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLog { get; set; }
    public int? idCliente { get; set; }
    public Guid? IdentityUserId { get; set; }
    public int? acao { get; set; }
    public int? idArea { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public DateTime? dataAgendamento { get; set; }
    public int? inssIdTipoBeneficio { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
