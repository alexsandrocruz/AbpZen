// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.BlogCategory;

/// <summary>
/// BlogCategory entity
/// </summary>
public class BlogCategory : AuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected BlogCategory()
    {
        // Required by EF Core
    }

    public BlogCategory(Guid id) : base(id)
    {
    }
}
