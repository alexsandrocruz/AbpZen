using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.UrlShorting;

[RequiresFeature(CmsKitProFeatures.UrlShortingEnable)]
[RequiresGlobalFeature(typeof(UrlShortingFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/url-shorting")]
[Authorize(CmsKitProAdminPermissions.UrlShorting.Default)]
public class UrlShortingAdminController : CmsKitProAdminController, IUrlShortingAdminAppService
{
    private readonly IUrlShortingAdminAppService _urlShortingAdminAppService;

    public UrlShortingAdminController(IUrlShortingAdminAppService urlShortingAdminAppService)
    {
        _urlShortingAdminAppService = urlShortingAdminAppService;
    }

    [HttpGet]
    public Task<PagedResultDto<ShortenedUrlDto>> GetListAsync(GetShortenedUrlListInput input)
    {
        return _urlShortingAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<ShortenedUrlDto> GetAsync(Guid id)
    {
        return _urlShortingAdminAppService.GetAsync(id);
    }

    [HttpPost]
    [Authorize(CmsKitProAdminPermissions.UrlShorting.Create)]
    public Task<ShortenedUrlDto> CreateAsync(CreateShortenedUrlDto input)
    {
        return _urlShortingAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(CmsKitProAdminPermissions.UrlShorting.Update)]
    public Task<ShortenedUrlDto> UpdateAsync(Guid id, UpdateShortenedUrlDto input)
    {
        return _urlShortingAdminAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(CmsKitProAdminPermissions.UrlShorting.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return _urlShortingAdminAppService.DeleteAsync(id);
    }
}
