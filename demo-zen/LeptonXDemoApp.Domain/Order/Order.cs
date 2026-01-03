using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Order;

/// <summary>
/// Order entity
/// </summary>
public class Order : FullAuditedAggregateRoot<Guid>
{
    public string? Number { get; set; }
    public DateTime? Date { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? CustomerId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.Customer.Customer? Customer { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.OrderItem.OrderItem> OrderItems { get; set; } = new List<LeptonXDemoApp.OrderItem.OrderItem>();

    protected Order()
    {
        // Required by EF Core
    }

    public Order(Guid id) : base(id)
    {
    }
}
