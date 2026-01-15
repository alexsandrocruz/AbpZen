using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Product.Dtos;

[Serializable]
public class ProductDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public double Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
