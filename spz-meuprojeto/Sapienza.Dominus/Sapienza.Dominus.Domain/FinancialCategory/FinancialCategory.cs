// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.FinancialCategory;

/// <summary>
/// FinancialCategory entity
/// </summary>
public class FinancialCategory : AuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Color { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.Transaction.Transaction> Transactions { get; set; } = new List<Dominus.Transaction.Transaction>();
    public virtual ICollection<Dominus.Budget.Budget> Budgets { get; set; } = new List<Dominus.Budget.Budget>();

    protected FinancialCategory()
    {
        // Required by EF Core
    }

    public FinancialCategory(Guid id) : base(id)
    {
    }
}
