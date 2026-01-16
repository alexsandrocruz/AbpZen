// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.RAGDoc;

/// <summary>
/// RAGDoc entity
/// </summary>
public class RAGDoc : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected RAGDoc()
    {
        // Required by EF Core
    }

    public RAGDoc(Guid id) : base(id)
    {
    }
}
