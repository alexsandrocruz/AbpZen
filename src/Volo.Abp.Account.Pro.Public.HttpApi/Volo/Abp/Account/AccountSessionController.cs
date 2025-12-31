using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Volo.Abp.Account;

[RemoteService(Name = AccountProPublicRemoteServiceConsts.RemoteServiceName)]
[Area(AccountProPublicRemoteServiceConsts.ModuleName)]
[ControllerName("Sessions")]
[Route("/api/account/sessions")]
public class AccountSessionController : AbpControllerBase, IAccountSessionAppService
{
    protected IAccountSessionAppService AccountSessionAppService { get; }

    public AccountSessionController(IAccountSessionAppService accountSessionAppService)
    {
        AccountSessionAppService = accountSessionAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetAccountIdentitySessionListInput input)
    {
        return AccountSessionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentitySessionDto> GetAsync(Guid id)
    {
        return AccountSessionAppService.GetAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task RevokeAsync(Guid id)
    {
        return AccountSessionAppService.RevokeAsync(id);
    }
}
