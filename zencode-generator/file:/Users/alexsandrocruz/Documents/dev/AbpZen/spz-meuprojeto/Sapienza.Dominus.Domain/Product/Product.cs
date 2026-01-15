// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Product;

/// <summary>
/// Product entity
/// </summary>
public class Product : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; }
    public double Price { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Product()
    {
        // Required by EF Core
    }

    public Product(Guid id) : base(id)
    {
    }
}
