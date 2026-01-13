using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.autoFTP.Dtos;

[Serializable]
public class autoFTPGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? id { get; set; }
    public string? arquivo { get; set; }
    public bool? processado { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
