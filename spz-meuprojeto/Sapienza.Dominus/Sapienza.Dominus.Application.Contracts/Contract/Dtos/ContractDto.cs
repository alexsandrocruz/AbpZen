using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Contract.Dtos;

[Serializable]
public class ContractDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public double TotalValue { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
