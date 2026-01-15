// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.TransactionAttachment;

/// <summary>
/// TransactionAttachment entity
/// </summary>
public class TransactionAttachment : Entity<Guid>
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? TransactionId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Transaction.Transaction? Transaction { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected TransactionAttachment()
    {
        // Required by EF Core
    }

    public TransactionAttachment(Guid id) : base(id)
    {
    }
}
