using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;

[Authorize(IdentityPermissions.Sessions.Default)]
public class IdentitySessionAppService : IdentityAppServiceBase,  IIdentitySessionAppService
{
    protected IdentitySessionManager IdentitySessionManager { get; }
    protected IIdentitySessionRepository IdentitySessionRepository { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }

    public IdentitySessionAppService(
        IdentitySessionManager identitySessionManager,
        IIdentitySessionRepository identitySessionRepository,
        IIdentityUserRepository identityUserRepository)
    {
        IdentitySessionManager = identitySessionManager;
        IdentitySessionRepository = identitySessionRepository;
        IdentityUserRepository = identityUserRepository;
    }

    public virtual async Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input)
    {
        var count = await IdentitySessionRepository.GetCountAsync(
            input.UserId,
            input.Device,
            input.ClientId
        );

        var sessions = await  IdentitySessionManager.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.UserId,
            input.Device,
            input.ClientId
        );

        var dtos = new List<IdentitySessionDto>(ObjectMapper.Map<List<IdentitySession>, List<IdentitySessionDto>>(sessions));
        var users = await IdentityUserRepository.GetListByIdsAsync(dtos.Select(x => x.UserId).ToArray());
        foreach (var dto in dtos)
        {
            dto.IsCurrent = dto.SessionId == CurrentUser.GetSessionId().ToString();
            if (dto.TenantId.HasValue)
            {
                dto.TenantName = CurrentTenant.Name;
            }

            dto.UserName = users.FirstOrDefault(x => x.Id == dto.UserId)?.UserName;
        }

        return new PagedResultDto<IdentitySessionDto>(count, dtos);
    }

    public virtual async Task<IdentitySessionDto> GetAsync(Guid id)
    {
        var session = await IdentitySessionManager.GetAsync(id);
        var dto = ObjectMapper.Map<IdentitySession, IdentitySessionDto>(session);
        if (dto.TenantId.HasValue)
        {
            dto.TenantName = CurrentTenant.Name;
        }
        var user = await IdentityUserRepository.GetAsync(dto.UserId);
        dto.UserName = user.UserName;
        return dto;
    }

    public virtual async Task RevokeAsync(Guid id)
    {
        await IdentitySessionManager.RevokeAsync(id);
    }
}
