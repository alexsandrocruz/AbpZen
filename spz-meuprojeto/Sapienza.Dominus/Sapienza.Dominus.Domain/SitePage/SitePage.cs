// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SitePage;

/// <summary>
/// SitePage entity
/// </summary>
public class SitePage : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SiteId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Site.Site? Site { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.SitePageVersion.SitePageVersion> SitePageVersions { get; set; } = new List<Dominus.SitePageVersion.SitePageVersion>();

    protected SitePage()
    {
        // Required by EF Core
    }

    public SitePage(Guid id) : base(id)
    {
    }
}
