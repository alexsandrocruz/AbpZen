using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.BlogPostVersion.Dtos;

[Serializable]
public class BlogPostVersionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? BlogPostId { get; set; }
}
