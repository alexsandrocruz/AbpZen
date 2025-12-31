using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

[Authorize(IdentityPermissions.SecurityLogs.Default)]
public class IdentitySecurityLogAppService : IdentityAppServiceBase, IIdentitySecurityLogAppService
{
    protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }

    public IdentitySecurityLogAppService(IIdentitySecurityLogRepository identitySecurityLogRepository)
    {
        IdentitySecurityLogRepository = identitySecurityLogRepository;
    }

    public virtual async Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(GetIdentitySecurityLogListInput input)
    {
        var securityLogs = await IdentitySecurityLogRepository.GetListAsync(
            sorting: input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId,
            clientIpAddress: input.ClientIpAddress
        );

        var totalCount = await IdentitySecurityLogRepository.GetCountAsync(
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId,
            clientIpAddress: input.ClientIpAddress
        );

        var securityLogDtos = ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogDto>>(securityLogs);
        return new PagedResultDto<IdentitySecurityLogDto>(totalCount, securityLogDtos);
    }

    public virtual async Task<IdentitySecurityLogDto> GetAsync(Guid id)
    {
        var securityLog = await IdentitySecurityLogRepository.GetAsync(id);
        return ObjectMapper.Map<IdentitySecurityLog, IdentitySecurityLogDto>(securityLog);
    }
}
