using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Product;

/// <summary>
/// Product entity
/// </summary>
public class Product : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }
    public string? Price { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? CategoryId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.Category.Category? Category { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.OrderItem.OrderItem> OrderItems { get; set; } = new List<LeptonXDemoApp.OrderItem.OrderItem>();

    protected Product()
    {
        // Required by EF Core
    }

    public Product(Guid id) : base(id)
    {
    }
}
