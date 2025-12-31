using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Payment.Plans;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host;

[Controller]
[RemoteService(Name = SaasHostRemoteServiceConsts.RemoteServiceName)]
[Area(SaasHostRemoteServiceConsts.ModuleName)]
[ControllerName("Edition")]
[Route("/api/saas/editions")]
public class EditionController : AbpControllerBase, IEditionAppService
{
    protected IEditionAppService Service { get; }

    public EditionController(IEditionAppService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<EditionDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<EditionDto>> GetListAsync(GetEditionsInput input)
    {
        return Service.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<EditionDto> CreateAsync(EditionCreateDto input)
    {
        return Service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
    {
        return Service.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpPut]
    [Route("{id}/move-all-tenants")]
    public virtual Task MoveAllTenantsAsync(Guid id, [FromQuery]Guid? editionId)
    {
        return Service.MoveAllTenantsAsync(id, editionId);
    }

    [HttpGet]
    [Route("all")]
    public virtual Task<List<EditionDto>> GetAllListAsync()
    {
        return Service.GetAllListAsync();
    }

    [HttpGet]
    [Route("statistics/usage-statistic")]
    public virtual Task<GetEditionUsageStatisticsResultDto> GetUsageStatisticsAsync()
    {
        return Service.GetUsageStatisticsAsync();
    }

    [HttpGet]
    [Route("plan-lookup")]
    public Task<List<PlanDto>> GetPlanLookupAsync()
    {
        return Service.GetPlanLookupAsync();
    }
}
