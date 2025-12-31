using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Admin.UrlShorting
{
    public class CreateShortenedUrlDto
    {
        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxSourceLength))]
        public string Source { get; set; }

        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxTargetLength))]
        public string Target { get; set; }
        
        public bool IsRegex { get; set; }
    }
}
