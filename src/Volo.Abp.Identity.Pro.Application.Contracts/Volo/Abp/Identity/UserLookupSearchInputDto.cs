using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class UserLookupSearchInputDto : ExtensiblePagedResultRequestDto, ISortedResultRequest
{
    public string Sorting { get; set; }

    public string Filter { get; set; }
}
