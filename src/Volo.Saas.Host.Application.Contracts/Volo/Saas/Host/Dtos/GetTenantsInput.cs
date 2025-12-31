using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Saas.Host.Dtos;

public class GetTenantsInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public bool GetEditionNames { get; set; } = true;

    public Guid? EditionId { get; set; }

    public DateTime? ExpirationDateMin { get; set; }

    public DateTime? ExpirationDateMax { get; set; }

    public TenantActivationState? ActivationState { get; set; }

    public DateTime? ActivationEndDateMin { get; set; }

    public DateTime? ActivationEndDateMax { get; set; }
}
