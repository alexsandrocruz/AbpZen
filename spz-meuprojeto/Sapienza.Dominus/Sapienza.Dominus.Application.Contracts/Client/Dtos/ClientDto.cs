using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Client.Dtos;

[Serializable]
public class ClientDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ClientType { get; set; }
    public string CompanyName { get; set; }
    public string CNPJ { get; set; }
    public string CPF { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
