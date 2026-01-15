using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ClientContact.Dtos;

[Serializable]
public class ClientContactDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
