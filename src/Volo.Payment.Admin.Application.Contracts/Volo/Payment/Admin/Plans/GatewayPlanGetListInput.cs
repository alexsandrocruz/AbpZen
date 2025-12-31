using System;
using Volo.Abp.Application.Dtos;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Plans;

public class GatewayPlanGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
