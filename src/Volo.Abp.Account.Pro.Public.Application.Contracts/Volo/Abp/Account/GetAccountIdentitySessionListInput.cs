using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Account;

public class GetAccountIdentitySessionListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Device { get; set; }

    public string ClientId { get; set; }
}
