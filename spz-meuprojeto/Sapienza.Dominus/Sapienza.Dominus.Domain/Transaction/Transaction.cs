// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Transaction;

/// <summary>
/// Transaction entity
/// </summary>
public class Transaction : FullAuditedAggregateRoot<Guid>
{
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime PaymentDate { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }
    public Guid? FinancialCategoryId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }
    public virtual Dominus.FinancialCategory.FinancialCategory? FinancialCategory { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.TransactionAttachment.TransactionAttachment> TransactionAttachments { get; set; } = new List<Dominus.TransactionAttachment.TransactionAttachment>();

    protected Transaction()
    {
        // Required by EF Core
    }

    public Transaction(Guid id) : base(id)
    {
    }
}
