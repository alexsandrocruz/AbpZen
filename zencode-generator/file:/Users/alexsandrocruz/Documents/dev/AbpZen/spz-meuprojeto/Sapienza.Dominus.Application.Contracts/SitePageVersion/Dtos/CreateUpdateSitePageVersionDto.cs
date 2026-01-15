using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.SitePageVersion.Dtos;

[Serializable]
public class CreateUpdateSitePageVersionDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public int VersionNumber { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SitePageId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
