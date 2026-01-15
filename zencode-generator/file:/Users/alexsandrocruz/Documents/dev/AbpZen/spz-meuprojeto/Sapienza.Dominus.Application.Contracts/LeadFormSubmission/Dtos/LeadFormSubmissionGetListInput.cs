using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadFormSubmission.Dtos;

[Serializable]
public class LeadFormSubmissionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadFormId { get; set; }
}
