using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Newsletters;

public class GetNewsletterRecordsRequestInput : PagedResultRequestDto
{
    public string Preference { get; set; }

    public string Source { get; set; }
    
    public string EmailAddress { get; set; }
}
