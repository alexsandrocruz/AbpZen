using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Volo.Abp.Gdpr;

[RemoteService(Name = GdprRemoteServiceConsts.RemoteServiceName)]
[Area(GdprRemoteServiceConsts.ModuleName)]
[Route("api/gdpr/requests")]
[ControllerName("GdprRequest")]
public class GdprRequestController : AbpControllerBase, IGdprRequestAppService
{
    protected IGdprRequestAppService GdprRequestAppService { get; }

    public GdprRequestController(IGdprRequestAppService gdprRequestAppService)
    {
        GdprRequestAppService = gdprRequestAppService;
    }

    [HttpPost("prepare-data")]
    public virtual Task PrepareUserDataAsync()
    {
        return GdprRequestAppService.PrepareUserDataAsync();
    }

    [HttpGet("download-token")]
    public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync(Guid id)
    {
        return GdprRequestAppService.GetDownloadTokenAsync(id);
    }

    [HttpGet("data/{requestId}")]
    public virtual async Task<IRemoteStreamContent> GetUserDataAsync(Guid requestId, string token)
    {
        var userDataStreamContent = await GdprRequestAppService.GetUserDataAsync(requestId, token);
        
        Response.Headers.Add("Content-Disposition", $"attachment;filename=\"{userDataStreamContent.FileName}\"");
        Response.Headers.Add("Accept-Ranges", "bytes");
        Response.ContentType = userDataStreamContent.ContentType;
        
        return userDataStreamContent;
    }

    [HttpGet("is-request-allowed")]
    public virtual Task<bool> IsNewRequestAllowedAsync()
    {
        return GdprRequestAppService.IsNewRequestAllowedAsync();
    }

    [HttpGet("list")]
    public virtual Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestInput input)
    {
        return GdprRequestAppService.GetListAsync(input);
    }

    [HttpDelete]
    public virtual Task DeleteUserDataAsync()
    {
        return GdprRequestAppService.DeleteUserDataAsync();
    }
}