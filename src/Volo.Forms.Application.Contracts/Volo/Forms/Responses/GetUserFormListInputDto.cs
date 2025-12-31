using Volo.Abp.Application.Dtos;

namespace Volo.Forms.Responses;

public class GetUserFormListInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
