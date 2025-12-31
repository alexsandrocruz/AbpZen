using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Volo.Abp.Gdpr;

[Authorize]
public class GdprRequestAppService : GdprAppServiceBase, IGdprRequestAppService
{
    protected IGdprRequestRepository GdprRequestRepository { get; }
    protected IDistributedEventBus EventBus { get; }
    protected AbpGdprOptions GdprOptions { get; }
    protected IDistributedCache<DownloadTokenCacheItem, string> DownloadTokenCache { get; }

    public GdprRequestAppService(
        IGdprRequestRepository gdprRequestRepository,
        IDistributedEventBus eventBus,
        IOptions<AbpGdprOptions> gdprOptions,
        IDistributedCache<DownloadTokenCacheItem, string> downloadTokenCache)
    {
        GdprRequestRepository = gdprRequestRepository;
        EventBus = eventBus;
        GdprOptions = gdprOptions.Value;
        DownloadTokenCache = downloadTokenCache;
    }
    
    public virtual async Task PrepareUserDataAsync()
    {
        if (!await IsNewRequestAllowedInternalAsync())
        {
            throw new BusinessException(GdprErrorCodes.NotAllowedForRequest);
        }

        var gdprRequest = new GdprRequest(GuidGenerator.Create(), CurrentUser.GetId(), Clock.Now.AddMinutes(GdprOptions.MinutesForDataPreparation));
        
        await GdprRequestRepository.InsertAsync(gdprRequest);

        await EventBus.PublishAsync(new GdprUserDataRequestedEto 
            {
                UserId = CurrentUser.GetId(),
                RequestId = gdprRequest.Id
            }
        );
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetUserDataAsync(Guid id, string token)
    {
        var downloadToken = await DownloadTokenCache.GetAsync(token);
        if (downloadToken == null || downloadToken.RequestId != id)
        {
            throw new AbpAuthorizationException();
        }

        await DownloadTokenCache.RemoveAsync(token);
        
        var request = await GdprRequestRepository.GetAsync(id, true);
        if (Clock.Now < request.ReadyTime)
        {
            throw new BusinessException(GdprErrorCodes.DataNotPreparedYet)
                .WithData("GdprDataReadyTime", request.ReadyTime.ToShortTimeString());
        }

        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var infos = request.Infos;
                foreach (var info in infos)
                {
                    var file = archive.CreateEntry(GuidGenerator.Create() + ".json", CompressionLevel.Fastest);
                    using (var entry = file.Open())
                    {
                        var byteArr = Encoding.UTF8.GetBytes(info.Data);
                        await entry.WriteAsync(byteArr, 0, byteArr.Length);
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            var ms = new MemoryStream();
            await memoryStream.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(ms, "PersonalData.zip", "application/zip");
        }
    }

    public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync(Guid requestId)
    {
        var request = await GdprRequestRepository.GetAsync(requestId);
        if (request.UserId != CurrentUser.GetId())
        {
            throw new UserFriendlyException(L["CanNotGetDownloadToken"]);
        }

        var token = Guid.NewGuid().ToString();

        await DownloadTokenCache.SetAsync(
            token,
            new DownloadTokenCacheItem { RequestId = requestId },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

        return new DownloadTokenResultDto
        {
            Token = token
        };
    }

    public virtual async Task<bool> IsNewRequestAllowedAsync()
    {
        return await IsNewRequestAllowedInternalAsync();
    }

    public virtual async Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestInput input)
    {
        if (input.UserId != CurrentUser.GetId())
        {
            throw new AbpAuthorizationException();
        }

        var count = await GdprRequestRepository.GetCountByUserIdAsync(input.UserId);
        var requests = await GdprRequestRepository.GetListAsync(input.UserId, input.Sorting, input.MaxResultCount, input.SkipCount);
        
        return new PagedResultDto<GdprRequestDto>(count, ObjectMapper.Map<List<GdprRequest>, List<GdprRequestDto>>(requests));
    }

    public virtual async Task DeleteUserDataAsync()
    {
        var userId = CurrentUser.GetId();
        var gdprRequests = await GdprRequestRepository.GetListAsync(userId);

        await GdprRequestRepository.DeleteManyAsync(gdprRequests, autoSave: true);
        
        await EventBus.PublishAsync(new GdprUserDataDeletionRequestedEto
        {
            UserId = userId
        });
    }

    private async Task<bool> IsNewRequestAllowedInternalAsync()
    {
        var latestRequestTime = await GdprRequestRepository.FindLatestRequestTimeOfUserAsync(CurrentUser.GetId());

        return latestRequestTime.HasValue && Clock.Now - latestRequestTime > GdprOptions.RequestTimeInterval;
    }
}