// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Comment;

/// <summary>
/// Comment entity
/// </summary>
public class Comment : FullAuditedEntity<Guid>
{
    public string EntityType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Comment()
    {
        // Required by EF Core
    }

    public Comment(Guid id) : base(id)
    {
    }
}
