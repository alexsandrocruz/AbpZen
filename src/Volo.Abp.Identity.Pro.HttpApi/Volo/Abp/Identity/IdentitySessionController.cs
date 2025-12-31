using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityProRemoteServiceConsts.ModuleName)]
[ControllerName("Sessions")]
[Route("/api/identity/sessions")]
public class IdentitySessionController : AbpControllerBase, IIdentitySessionAppService
{
    protected IIdentitySessionAppService IdentitySessionAppService { get; }

    public IdentitySessionController(IIdentitySessionAppService identitySessionAppService)
    {
        IdentitySessionAppService = identitySessionAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input)
    {
        return IdentitySessionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentitySessionDto> GetAsync(Guid id)
    {
        return IdentitySessionAppService.GetAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task RevokeAsync(Guid id)
    {
        return IdentitySessionAppService.RevokeAsync(id);
    }
}
