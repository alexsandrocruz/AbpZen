using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Volo.Payment.Plans;

public class GatewayPlanDto : ExtensibleEntityDto
{
    public Guid PlanId { get; set; }

    public string Gateway { get; set; }

    public string ExternalId { get; set; }

    public GatewayPlanDto()
    {
        ExtraProperties = new();
    }
}
