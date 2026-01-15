// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SitePageVersion;

/// <summary>
/// SitePageVersion entity
/// </summary>
public class SitePageVersion : Entity<Guid>
{
    public int VersionNumber { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SitePageId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.SitePage.SitePage? SitePage { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SitePageVersion()
    {
        // Required by EF Core
    }

    public SitePageVersion(Guid id) : base(id)
    {
    }
}
