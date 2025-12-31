using System;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqQuestionListFilterDto : PagedAndSortedResultRequestDto
{
    public Guid SectionId { get; set; }

    [CanBeNull] 
    public string Filter { get; set; }
}