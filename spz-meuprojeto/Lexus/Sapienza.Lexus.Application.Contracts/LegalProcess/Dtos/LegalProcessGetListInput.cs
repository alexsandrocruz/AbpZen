#nullable enable
using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.LegalProcess.Dtos;

[Serializable]
public class LegalProcessGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? ProcessNumber { get; set; }
    public string? Title { get; set; }
    public DateTime? DateOpened { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LawyerId { get; set; }
    public Guid? ClientId { get; set; }
}
