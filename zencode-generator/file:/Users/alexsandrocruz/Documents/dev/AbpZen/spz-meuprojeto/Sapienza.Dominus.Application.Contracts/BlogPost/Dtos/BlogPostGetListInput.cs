using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.BlogPost.Dtos;

[Serializable]
public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SiteId { get; set; }
}
