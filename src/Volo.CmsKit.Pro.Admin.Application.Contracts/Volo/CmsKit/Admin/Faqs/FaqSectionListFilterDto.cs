using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqSectionListFilterDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
