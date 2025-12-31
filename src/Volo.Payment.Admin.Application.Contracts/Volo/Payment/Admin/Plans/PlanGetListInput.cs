using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Payment.Admin.Plans;

[Serializable]
public class PlanGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
