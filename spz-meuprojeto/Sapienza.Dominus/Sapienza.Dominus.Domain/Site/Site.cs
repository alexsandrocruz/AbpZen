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
    public virtual ICollection<Sapienza.Dominus.SitePage.SitePage> SitePages { get; set; } = new List<Sapienza.Dominus.SitePage.SitePage>();
    public virtual ICollection<Sapienza.Dominus.BlogPost.BlogPost> BlogPosts { get; set; } = new List<Sapienza.Dominus.BlogPost.BlogPost>();
    public virtual ICollection<Sapienza.Dominus.SiteVisitEvent.SiteVisitEvent> SiteVisitEvents { get; set; } = new List<Sapienza.Dominus.SiteVisitEvent.SiteVisitEvent>();
    public virtual ICollection<Sapienza.Dominus.SiteVisitDailyStat.SiteVisitDailyStat> SiteVisitDailyStats { get; set; } = new List<Sapienza.Dominus.SiteVisitDailyStat.SiteVisitDailyStat>();

    protected Site()
    {
        // Required by EF Core
    }

    public Site(Guid id) : base(id)
    {
    }
}
