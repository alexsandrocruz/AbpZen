using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.CustomFieldValue.Dtos;

[Serializable]
public class CustomFieldValueGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? CustomFieldId { get; set; }
}
