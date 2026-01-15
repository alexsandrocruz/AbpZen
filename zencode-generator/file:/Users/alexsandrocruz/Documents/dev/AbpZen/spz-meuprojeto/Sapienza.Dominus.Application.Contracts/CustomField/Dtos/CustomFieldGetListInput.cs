using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.CustomField.Dtos;

[Serializable]
public class CustomFieldGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
