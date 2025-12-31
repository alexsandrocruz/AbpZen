using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict;

[RemoteService(Name = AbpOpenIddictProRemoteServiceConsts.RemoteServiceName)]
[Area(AbpOpenIddictProRemoteServiceConsts.ModuleName)]
[Controller]
[ControllerName("Scopes")]
[Route("api/openiddict/scopes")]
[DisableAuditing]
public class ScopeController : AbpOpenIddictProController, IScopeAppService
{
    protected IScopeAppService ScopeAppService { get; }
    
    public ScopeController(IScopeAppService scopeAppService)
    {
        ScopeAppService = scopeAppService;
    }
    
    [HttpGet]
    [Route("{id}")]
    public virtual Task<ScopeDto> GetAsync(Guid id)
    {
        return ScopeAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ScopeDto>> GetListAsync(GetScopeListInput input)
    {
        return ScopeAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<ScopeDto> CreateAsync(CreateScopeInput input)
    {
        return ScopeAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ScopeDto> UpdateAsync(Guid id, UpdateScopeInput input)
    {
        return ScopeAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync(Guid id)
    {
        return ScopeAppService.DeleteAsync(id);
    }
    
    [HttpGet]
    [Route("all")]
    public Task<List<ScopeDto>> GetAllScopesAsync()
    {
        return ScopeAppService.GetAllScopesAsync();
    }
}
