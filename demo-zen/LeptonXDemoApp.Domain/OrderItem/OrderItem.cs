using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.OrderItem;

/// <summary>
/// OrderItem entity
/// </summary>
public class OrderItem : FullAuditedAggregateRoot<Guid>
{
    public decimal? Quant { get; set; }
    public decimal? Price { get; set; }
    public decimal? Total { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProductId { get; set; }
    public Guid? OrderId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.Product.Product? Product { get; set; }
    public virtual LeptonXDemoApp.Order.Order? Order { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected OrderItem()
    {
        // Required by EF Core
    }

    public OrderItem(Guid id) : base(id)
    {
    }
}
