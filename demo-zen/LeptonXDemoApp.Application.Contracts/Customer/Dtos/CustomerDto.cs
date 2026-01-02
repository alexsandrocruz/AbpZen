using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Customer.Dtos;

[Serializable]
public class CustomerDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
}
