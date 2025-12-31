using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityProRemoteServiceConsts.ModuleName)]
[ControllerName("SecurityLog")]
[Route("api/identity/security-logs")]
public class IdentitySecurityLogController : AbpControllerBase, IIdentitySecurityLogAppService
{
    protected IIdentitySecurityLogAppService IdentitySecurityLogAppService { get; }

    public IdentitySecurityLogController(IIdentitySecurityLogAppService identitySecurityLogAppService)
    {
        IdentitySecurityLogAppService = identitySecurityLogAppService;
    }

    [HttpGet]
    public Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync([FromQuery] GetIdentitySecurityLogListInput input)
    {
        return IdentitySecurityLogAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<IdentitySecurityLogDto> GetAsync(Guid id)
    {
        return IdentitySecurityLogAppService.GetAsync(id);
    }
}
