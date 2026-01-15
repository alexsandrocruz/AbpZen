using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Comment.Dtos;

[Serializable]
public class CommentGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
