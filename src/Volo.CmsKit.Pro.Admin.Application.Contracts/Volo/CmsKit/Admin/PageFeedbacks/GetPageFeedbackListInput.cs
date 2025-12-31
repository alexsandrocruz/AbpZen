using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.PageFeedbacks;

public class GetPageFeedbackListInput : PagedAndSortedResultRequestDto
{
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public bool? IsHandled { get; set; }
    public bool? IsUseful { get; set; }
    public string Url { get; set; }
    public bool? HasUserNote { get; set; }
    public bool? HasAdminNote { get; set; }
}