using System;

namespace Volo.CmsKit.UrlShorting
{
    [Serializable]
    public class ShortenedUrlCacheItem
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }
        
        public bool IsRegex { get; set; }

        public bool Exists { get; set; } = true;
    }
}
