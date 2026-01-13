using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Client.Dtos;

[Serializable]
public class ClientDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public ClientType Type { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Notes { get; set; }
    public bool? IsActive { get; set; }
    public string? LeadStatus { get; set; }
    public DateTime? FirstContactDate { get; set; }
    public DateTime? LastContactDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
