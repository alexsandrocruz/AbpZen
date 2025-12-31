using Volo.Abp.Domain.Entities;

namespace Volo.Saas.Host.Dtos;

public class SaasTenantUpdateDto : SaasTenantCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
