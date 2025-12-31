using Volo.Abp.Application.Dtos;

namespace Volo.Forms.Forms;

public class GetFormListInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
