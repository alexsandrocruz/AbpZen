using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.UrlShorting;

[Serializable]
public class ShortenedUrlAlreadyExistsException : BusinessException
{
    public ShortenedUrlAlreadyExistsException([NotNull] string source)
    : base(CmsKitProErrorCodes.UrlShorting.ShortenedUrlAlreadyExistsException)
    {
        WithData(nameof(ShortenedUrl.Source), source);
    }
}
