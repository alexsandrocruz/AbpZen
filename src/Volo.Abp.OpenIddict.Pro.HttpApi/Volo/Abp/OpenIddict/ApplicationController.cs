using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Applications.Dtos;

namespace Volo.Abp.OpenIddict;

[RemoteService(Name = AbpOpenIddictProRemoteServiceConsts.RemoteServiceName)]
[Area(AbpOpenIddictProRemoteServiceConsts.ModuleName)]
[Controller]
[ControllerName("Applications")]
[Route("api/openiddict/applications")]
[DisableAuditing]
public class ApplicationController : AbpOpenIddictProController, IApplicationAppService
{
    protected IApplicationAppService ApplicationAppService { get; }

    public ApplicationController(IApplicationAppService applicationAppService)
    {
        ApplicationAppService = applicationAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ApplicationDto> GetAsync(Guid id)
    {
        return ApplicationAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ApplicationDto>> GetListAsync(GetApplicationListInput input)
    {
        return ApplicationAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<ApplicationDto> CreateAsync(CreateApplicationInput input)
    {
        return ApplicationAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ApplicationDto> UpdateAsync(Guid id, UpdateApplicationInput input)
    {
        return ApplicationAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync(Guid id)
    {
        return ApplicationAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}/token-lifetime")]
    public virtual Task<ApplicationTokenLifetimeDto> GetTokenLifetimeAsync(Guid id)
    {
        return ApplicationAppService.GetTokenLifetimeAsync(id);
    }

    [HttpPut]
    [Route("{id}/token-lifetime")]
    public virtual Task SetTokenLifetimeAsync(Guid id, ApplicationTokenLifetimeDto input)
    {
        return ApplicationAppService.SetTokenLifetimeAsync(id, input);
    }
}
