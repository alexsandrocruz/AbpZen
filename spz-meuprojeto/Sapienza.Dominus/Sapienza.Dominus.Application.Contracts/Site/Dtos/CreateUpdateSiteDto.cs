using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Site.Dtos;

[Serializable]
public class CreateUpdateSiteDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Slug { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
