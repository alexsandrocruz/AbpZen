using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Product.Dtos;

[Serializable]
public class ProductDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CategoryId { get; set; }
    public string? CategoryDisplayName { get; set; }
}
