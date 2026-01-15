// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Site;

/// <summary>
/// Site entity
/// </summary>
public class Site : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Status { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.SitePage.SitePage> SitePages { get; set; } = new List<Dominus.SitePage.SitePage>();
    public virtual ICollection<Dominus.BlogPost.BlogPost> BlogPosts { get; set; } = new List<Dominus.BlogPost.BlogPost>();
    public virtual ICollection<Dominus.SiteVisitEvent.SiteVisitEvent> SiteVisitEvents { get; set; } = new List<Dominus.SiteVisitEvent.SiteVisitEvent>();
    public virtual ICollection<Dominus.SiteVisitDailyStat.SiteVisitDailyStat> SiteVisitDailyStats { get; set; } = new List<Dominus.SiteVisitDailyStat.SiteVisitDailyStat>();

    protected Site()
    {
        // Required by EF Core
    }

    public Site(Guid id) : base(id)
    {
    }
}
