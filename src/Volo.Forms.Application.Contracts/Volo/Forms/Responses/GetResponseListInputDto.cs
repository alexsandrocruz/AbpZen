using Volo.Abp.Application.Dtos;

namespace Volo.Forms.Responses;

public class GetResponseListInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
