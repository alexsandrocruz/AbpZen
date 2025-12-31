using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.UrlShorting
{
    public class GetShortenedUrlListInput : PagedAndSortedResultRequestDto
    {
        public string ShortenedUrlFilter { get; set; }
    }
}
