using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Plans;
using Volo.Saas.Editions;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Tenants;

namespace Volo.Saas.Host;

[Authorize(SaasHostPermissions.Editions.Default)]
public class EditionAppService : SaasHostAppServiceBase, IEditionAppService
{
    protected IEditionRepository EditionRepository { get; }
    protected EditionManager EditionManager { get; }
    protected ITenantRepository TenantRepository { get; }
    protected IPlanAppService PlanAppService { get; }
    protected AbpSaasPaymentOptions PaymentOptions { get; }

    public EditionAppService(
        IEditionRepository editionRepository,
        EditionManager editionManager,
        ITenantRepository tenantRepository,
        IServiceProvider serviceProvider,
        IOptions<AbpSaasPaymentOptions> paymentOptions)
    {
        EditionRepository = editionRepository;
        EditionManager = editionManager;
        TenantRepository = tenantRepository;
        PlanAppService = serviceProvider.GetService<IPlanAppService>();
        PaymentOptions = paymentOptions.Value;
    }

    public virtual async Task<EditionDto> GetAsync(Guid id)
    {
        var edition = await EditionRepository.GetAsync(id);

        var editionDto = ObjectMapper.Map<Edition, EditionDto>(edition);

        if (PaymentOptions.IsPaymentSupported && edition.PlanId.HasValue)
        {
            var plan = await PlanAppService.GetAsync(edition.PlanId.Value);
            editionDto.PlanName = plan?.Name;
        }

        editionDto.TenantCount = await TenantRepository.GetCountAsync(editionId: edition.Id);

        return editionDto;
    }

    public virtual async Task<PagedResultDto<EditionDto>> GetListAsync(GetEditionsInput input)
    {
        var count = await EditionRepository.GetCountAsync(input.Filter);
        var editions = await EditionRepository.GetListWithTenantCountAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

        var editionDtos = ObjectMapper.Map<List<Edition>, List<EditionDto>>(editions.Select(x => x.Edition).ToList());
        foreach (var editionDto in editionDtos)
        {
            editionDto.TenantCount = editions.FirstOrDefault(x => x.Edition.Id == editionDto.Id)?.TenantCount ?? 0;
        }

        if (PaymentOptions.IsPaymentSupported)
        {
            var planIds = editions
                .Select(x => x.Edition)
                .Where(x => x.PlanId.HasValue)
                .Select(x => x.PlanId.Value)
                .Distinct()
                .ToArray();

            var plans = await PlanAppService.GetManyAsync(planIds);

            foreach (var editionDto in editionDtos.Where(x => x.PlanId.HasValue))
            {
                var plan = plans.FirstOrDefault(x => x.Id == editionDto.PlanId.Value);

                editionDto.PlanName = plan?.Name;
            }
        }

        return new PagedResultDto<EditionDto>(
            count,
            editionDtos
        );
    }

    [Authorize(SaasHostPermissions.Editions.Create)]
    public virtual async Task<EditionDto> CreateAsync(EditionCreateDto input)
    {
        var edition = await EditionManager.CreateAsync(input.DisplayName);
        edition.PlanId = input.PlanId;

        input.MapExtraPropertiesTo(edition);

        await EditionRepository.InsertAsync(edition);

        return ObjectMapper.Map<Edition, EditionDto>(edition);
    }

    [Authorize(SaasHostPermissions.Editions.Update)]
    public virtual async Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
    {
        var edition = await EditionRepository.GetAsync(id);

        await EditionManager.ChangeDisplayNameAsync(edition, input.DisplayName);
        
        edition.PlanId = input.PlanId;
        edition.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        input.MapExtraPropertiesTo(edition);

        var updatedEdition = await EditionRepository.UpdateAsync(edition);

        return ObjectMapper.Map<Edition, EditionDto>(updatedEdition);
    }

    [Authorize(SaasHostPermissions.Editions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var edition = await EditionRepository.GetAsync(id);
        await EditionManager.DeleteAsync(edition, null);
    }

    [Authorize(SaasHostPermissions.Editions.Update)]
    public virtual async Task MoveAllTenantsAsync(Guid id, Guid? targetEditionId)
    {
        var edition = await EditionRepository.GetAsync(id);
        await EditionManager.MoveAllTenantsAsync(edition.Id, targetEditionId);
    }

    public virtual async Task<List<EditionDto>> GetAllListAsync()
    {
        var editions = await EditionRepository.GetListWithTenantCountAsync();
        var dtos = ObjectMapper.Map<List<Edition>, List<EditionDto>>(editions.Select(x => x.Edition).ToList());
        foreach (var dto in dtos)
        {
            dto.TenantCount = editions.FirstOrDefault(x => x.Edition.Id == dto.Id)?.TenantCount ?? 0;
        }
        return dtos;
    }

    public virtual async Task<GetEditionUsageStatisticsResultDto> GetUsageStatisticsAsync()
    {
        var editions = await EditionRepository.GetListAsync();
        var tenants = await TenantRepository.GetListAsync();

        var result = tenants.GroupBy(info => info.GetActiveEditionId())
            .Select(group => new {
                EditionId = group.Key,
                Count = group.Count()
            });

        var data = new Dictionary<string, int>();

        foreach (var element in result)
        {
            var displayName = editions.FirstOrDefault(e => e.Id == element.EditionId)?.DisplayName;

            if (displayName != null)
            {
                data.Add(displayName, element.Count);
            }
        }

        return new GetEditionUsageStatisticsResultDto()
        {
            Data = data
        };
    }

    [Obsolete("Use `IPlanAppService` to perform this operation. This will be removed in next major version.")]
    public virtual Task<List<PlanDto>> GetPlanLookupAsync()
    {
        if (PaymentOptions.IsPaymentSupported)
        {
            return PlanAppService.GetPlanListAsync();
        }

        return Task.FromResult(new List<PlanDto>());
    }
}
