// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Budget;

/// <summary>
/// Budget entity
/// </summary>
public class Budget : FullAuditedEntity<Guid>
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double Amount { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? FinancialCategoryId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.FinancialCategory.FinancialCategory? FinancialCategory { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Budget()
    {
        // Required by EF Core
    }

    public Budget(Guid id) : base(id)
    {
    }
}
