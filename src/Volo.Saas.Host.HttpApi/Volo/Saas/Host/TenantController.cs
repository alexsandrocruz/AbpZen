using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host;

[Controller]
[RemoteService(Name = SaasHostRemoteServiceConsts.RemoteServiceName)]
[Area(SaasHostRemoteServiceConsts.ModuleName)]
[ControllerName("Tenant")]
[Route("/api/saas/tenants")]
public class TenantController : AbpControllerBase, ITenantAppService
{
    protected ITenantAppService Service { get; }

    public TenantController(ITenantAppService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SaasTenantDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SaasTenantDto>> GetListAsync(GetTenantsInput input)
    {
        return Service.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<SaasTenantDto> CreateAsync(SaasTenantCreateDto input)
    {
        ValidateModel();
        return Service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<SaasTenantDto> UpdateAsync(Guid id, SaasTenantUpdateDto input)
    {
        return Service.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("databases")]
    public Task<SaasTenantDatabasesDto> GetDatabasesAsync()
    {
        return Service.GetDatabasesAsync();
    }

    [HttpGet]
    [Route("{id}/connection-strings")]
    public Task<SaasTenantConnectionStringsDto> GetConnectionStringsAsync(Guid id)
    {
        return Service.GetConnectionStringsAsync(id);
    }

    [HttpPut]
    [Route("{id}/connection-strings")]
    public Task UpdateConnectionStringsAsync(Guid id, SaasTenantConnectionStringsDto input)
    {
        return Service.UpdateConnectionStringsAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/apply-database-migrations")]
    public Task ApplyDatabaseMigrationsAsync(Guid id)
    {
        return Service.ApplyDatabaseMigrationsAsync(id);
    }

    [HttpGet]
    [Route("lookup/editions")]
    public Task<List<EditionLookupDto>> GetEditionLookupAsync()
    {
        return Service.GetEditionLookupAsync();
    }

    [HttpGet]
    [Route("check-connection-string")]
    public Task<bool> CheckConnectionStringAsync(string connectionString)
    {
        return Service.CheckConnectionStringAsync(connectionString);
    }

    [HttpPut]
    [Route("{id}/set-password")]
    public Task SetPasswordAsync(Guid id, SaasTenantSetPasswordDto input)
    {
        return Service.SetPasswordAsync(id, input);
    }
}
