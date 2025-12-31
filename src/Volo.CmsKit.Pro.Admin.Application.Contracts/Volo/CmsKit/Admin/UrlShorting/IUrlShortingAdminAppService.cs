using System;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.UrlShorting
{
    public interface IUrlShortingAdminAppService
        : ICrudAppService<
            ShortenedUrlDto,
            ShortenedUrlDto,
            Guid,
            GetShortenedUrlListInput,
            CreateShortenedUrlDto,
            UpdateShortenedUrlDto
        >
    {
    }
}
