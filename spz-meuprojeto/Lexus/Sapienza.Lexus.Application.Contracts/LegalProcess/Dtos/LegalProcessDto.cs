using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.LegalProcess.Dtos;

[Serializable]
public class LegalProcessDto : FullAuditedEntityDto<Guid>
{
    public string ProcessNumber { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateOpened { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid LawyerId { get; set; }
    public string? LawyerDisplayName { get; set; }
    public Guid ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
