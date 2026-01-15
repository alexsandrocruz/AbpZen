using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.SitePage.Dtos;

[Serializable]
public class CreateUpdateSitePageDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Slug { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SiteId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
