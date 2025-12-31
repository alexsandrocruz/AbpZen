using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.Abp.Gdpr;

public interface IGdprRequestAppService : IApplicationService
{
    Task PrepareUserDataAsync();

    Task<IRemoteStreamContent> GetUserDataAsync(Guid requestId, string token);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync(Guid id);

    Task<bool> IsNewRequestAllowedAsync();

    Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestInput input);

    Task DeleteUserDataAsync();
}