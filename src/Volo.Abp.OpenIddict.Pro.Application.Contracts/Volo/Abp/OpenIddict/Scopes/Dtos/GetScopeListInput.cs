using Volo.Abp.Application.Dtos;

namespace Volo.Abp.OpenIddict.Scopes.Dtos;

public class GetScopeListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
