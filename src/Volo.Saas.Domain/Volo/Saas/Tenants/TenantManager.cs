using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace Volo.Saas.Tenants;

public class TenantManager : DomainService, ITenantManager
{
    protected ITenantRepository TenantRepository { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected ILocalEventBus LocalEventBus { get; }

    public TenantManager(
        ITenantRepository tenantRepository,
        ITenantNormalizer tenantNormalizer,
        ILocalEventBus localEventBus)
    {
        TenantRepository = tenantRepository;
        TenantNormalizer = tenantNormalizer;
        LocalEventBus = localEventBus;
    }

    public virtual async Task<Tenant> CreateAsync(string name, Guid? editionId = null)
    {
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name);
        await ValidateNameAsync(normalizedName);
        return new Tenant(GuidGenerator.Create(), name, normalizedName, editionId);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name);

        await ValidateNameAsync(normalizedName, tenant.Id);
        await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));
        tenant.SetName(name);
        tenant.SetNormalizedName(normalizedName);
    }

    protected virtual async Task ValidateNameAsync(string normalizeName, Guid? expectedId = null)
    {
        var tenant = await TenantRepository.FindByNameAsync(normalizeName);
        if (tenant != null && tenant.Id != expectedId)
        {
            throw new BusinessException("Volo.Saas:DuplicateTenantName").WithData("Name", normalizeName);
        }
    }

    public virtual Task<bool> IsActiveAsync(Tenant tenant)
    {
        return Task.FromResult(tenant.ActivationState switch
        {
            TenantActivationState.Active => true,
            TenantActivationState.Passive => false,
            TenantActivationState.ActiveWithLimitedTime => tenant.ActivationEndDate >= Clock.Now,
            _ => false
        });
    }

    [Obsolete("Use IsActiveAsync method.")]
    public bool IsActive(Tenant tenant)
    {
        return tenant.ActivationState switch
        {
            TenantActivationState.Active => true,
            TenantActivationState.Passive => false,
            TenantActivationState.ActiveWithLimitedTime => tenant.ActivationEndDate >= Clock.Now,
            _ => false
        };
    }
}
