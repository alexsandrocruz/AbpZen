using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadFormField.Dtos;

[Serializable]
public class LeadFormFieldGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadFormId { get; set; }
}
