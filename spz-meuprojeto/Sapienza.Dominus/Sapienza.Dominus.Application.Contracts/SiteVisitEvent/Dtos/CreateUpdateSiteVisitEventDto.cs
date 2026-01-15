using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.SiteVisitEvent.Dtos;

[Serializable]
public class CreateUpdateSiteVisitEventDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string VisitorId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SiteId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
