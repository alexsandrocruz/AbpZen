using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Category;

/// <summary>
/// Category entity
/// </summary>
public class Category : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.Product.Product> Products { get; set; } = new List<LeptonXDemoApp.Product.Product>();

    protected Category()
    {
        // Required by EF Core
    }

    public Category(Guid id) : base(id)
    {
    }
}
