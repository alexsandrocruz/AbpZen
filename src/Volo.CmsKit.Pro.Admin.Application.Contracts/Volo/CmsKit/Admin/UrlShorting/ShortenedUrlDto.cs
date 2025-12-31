using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.UrlShorting
{
    public class ShortenedUrlDto : CreationAuditedEntityDto<Guid>
    {
        public string Source { get; set; }

        public string Target { get; set; }
        
        public bool IsRegex { get; set; }
    }
}
