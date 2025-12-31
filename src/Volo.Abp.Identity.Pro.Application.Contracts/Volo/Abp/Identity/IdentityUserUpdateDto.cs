using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity;

public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public bool EmailConfirmed { get; set; }
        
    public bool PhoneNumberConfirmed { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}
