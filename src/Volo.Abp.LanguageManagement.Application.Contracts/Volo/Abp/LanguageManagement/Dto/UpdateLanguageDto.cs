using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.LanguageManagement.Dto;

public class UpdateLanguageDto : ExtensibleObject, IHasConcurrencyStamp
{
    public string DisplayName { get; set; }

    public bool IsEnabled { get; set; }

    public string ConcurrencyStamp { get; set; }

    public UpdateLanguageDto()
        : base(false)
    {

    }
}
