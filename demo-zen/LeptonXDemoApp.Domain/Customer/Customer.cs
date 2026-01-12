using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Customer;

/// <summary>
/// Customer entity
/// </summary>
public class Customer : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.Order.Order> Orders { get; set; } = new List<LeptonXDemoApp.Order.Order>();

    protected Customer()
    {
        // Required by EF Core
    }

    public Customer(Guid id) : base(id)
    {
    }
}
