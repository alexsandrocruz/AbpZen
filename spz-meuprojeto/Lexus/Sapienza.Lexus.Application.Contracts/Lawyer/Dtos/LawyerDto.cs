using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Sapienza.Lexus.Lawyer.Dtos;

[Serializable]
public class LawyerDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string FullName { get; set; }
    public string PreferredName { get; set; }
    public string ConcurrencyStamp { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
