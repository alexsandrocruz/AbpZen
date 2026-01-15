// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.BlogPostVersion;

/// <summary>
/// BlogPostVersion entity
/// </summary>
public class BlogPostVersion : Entity<Guid>
{
    public int VersionNumber { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? BlogPostId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.BlogPost.BlogPost? BlogPost { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected BlogPostVersion()
    {
        // Required by EF Core
    }

    public BlogPostVersion(Guid id) : base(id)
    {
    }
}
