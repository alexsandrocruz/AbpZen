using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SmsLog.Dtos;

[Serializable]
public class SmsLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
