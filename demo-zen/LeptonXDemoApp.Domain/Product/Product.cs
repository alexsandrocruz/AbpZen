using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using CategoryEntity = LeptonXDemoApp.Category.Category;

namespace LeptonXDemoApp.Product;

/// <summary>
/// Product entity
/// </summary>
public class Product : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public decimal? Price { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? CategoryId { get; set; }

    // ========== Navigation Properties ==========
    public virtual CategoryEntity? Category { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Product()
    {
        // Required by EF Core
    }

    public Product(Guid id) : base(id)
    {
    }
}
