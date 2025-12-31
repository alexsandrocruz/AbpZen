using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.UrlShorting
{
    public class ShortenedUrlDto
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }
        
        public bool IsRegex { get; set; }
    }
}
