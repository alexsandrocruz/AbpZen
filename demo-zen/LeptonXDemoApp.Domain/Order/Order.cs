using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Order;

/// <summary>
/// Order entity
/// </summary>
public class Order : FullAuditedEntity<Guid>
{
    public string Number { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public enum Status { get; set; }
    public string? Obs { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

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
