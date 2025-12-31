using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Polls;

[Serializable]
public class GetPollListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
