using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Order.Dtos;

[Serializable]
public class OrderDto : FullAuditedEntityDto<Guid>
{
    public string Number { get; set; }
    public DateTime? Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CustomerId { get; set; }
    public string? CustomerDisplayName { get; set; }
}
