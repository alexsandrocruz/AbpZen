using Volo.Abp.Domain.Entities;

namespace Volo.Saas.Host.Dtos;

public class EditionUpdateDto : EditionCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
