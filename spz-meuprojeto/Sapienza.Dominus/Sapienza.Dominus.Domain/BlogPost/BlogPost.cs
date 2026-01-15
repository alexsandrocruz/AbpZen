// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.BlogPost;

/// <summary>
/// BlogPost entity
/// </summary>
public class BlogPost : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SiteId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Site.Site? Site { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.BlogPostVersion.BlogPostVersion> BlogPostVersions { get; set; } = new List<Dominus.BlogPostVersion.BlogPostVersion>();

    protected BlogPost()
    {
        // Required by EF Core
    }

    public BlogPost(Guid id) : base(id)
    {
    }
}
