using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.UrlShorting
{
    public interface IUrlShortingPublicAppService : IApplicationService
    {
        Task<ShortenedUrlDto> FindBySourceAsync(string source);
    }
}
