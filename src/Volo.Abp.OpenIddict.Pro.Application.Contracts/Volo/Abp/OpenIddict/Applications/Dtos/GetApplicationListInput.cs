using Volo.Abp.Application.Dtos;

namespace Volo.Abp.OpenIddict.Applications.Dtos;

public class GetApplicationListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
