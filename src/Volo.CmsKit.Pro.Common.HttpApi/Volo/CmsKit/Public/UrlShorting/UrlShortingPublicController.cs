using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.UrlShorting;

[RequiresFeature(CmsKitProFeatures.UrlShortingEnable)]
[RequiresGlobalFeature(typeof(UrlShortingFeature))]
[RemoteService(Name = CmsKitProCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/url-shorting")]
public class UrlShortingPublicController : CmsKitProCommonController, IUrlShortingPublicAppService
{
    protected IUrlShortingPublicAppService UrlShortingPublicAppService { get; }

    public UrlShortingPublicController(IUrlShortingPublicAppService urlShortingPublicAppService)
    {
        UrlShortingPublicAppService = urlShortingPublicAppService;
    }

    [HttpGet]
    public virtual async Task<ShortenedUrlDto> FindBySourceAsync(string source)
    {
        return await UrlShortingPublicAppService.FindBySourceAsync(source);
    }
}
