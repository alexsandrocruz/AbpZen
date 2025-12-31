using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Saas.Host.Dtos;

public class EditionDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    public string DisplayName { get; set; }

    public Guid? PlanId { get; set; }

    public string PlanName { get; set; }

    public string ConcurrencyStamp { get; set; }

    public long TenantCount { get; set; }
}
