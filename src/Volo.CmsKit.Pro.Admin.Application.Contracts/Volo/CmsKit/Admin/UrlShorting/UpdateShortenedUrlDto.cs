using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Admin.UrlShorting
{
    public class UpdateShortenedUrlDto
    {
        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxTargetLength))]
        public string Target { get; set; }
    }
}
