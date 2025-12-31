using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Tenants;

namespace Volo.Saas.Features;

public class TenantEditionFeatureValueProvider : FeatureManagementProvider, ITransientDependency
{
    public override string Name => "TE";

    protected ICurrentTenant CurrentTenant { get; }
    protected ITenantRepository TenantRepository { get; }

    public TenantEditionFeatureValueProvider(
        IFeatureManagementStore store,
        ICurrentTenant currentTenant,
        ITenantRepository tenantRepository)
        : base(store)
    {
        CurrentTenant = currentTenant;
        TenantRepository = tenantRepository;
    }

    public override bool Compatible(string providerName)
    {
        return providerName == TenantFeatureValueProvider.ProviderName || base.Compatible(providerName);
    }

    public override async Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
    {
        return await Store.GetOrNullAsync(feature.Name, EditionFeatureValueProvider.ProviderName, await NormalizeProviderKeyAsync(providerKey));
    }

    public override async Task SetAsync(FeatureDefinition feature, string value, string providerKey)
    {
        await Store.SetAsync(feature.Name, value, EditionFeatureValueProvider.ProviderName, await NormalizeProviderKeyAsync(providerKey));
    }

    public override async Task ClearAsync(FeatureDefinition feature, string providerKey)
    {
        await Store.DeleteAsync(feature.Name, EditionFeatureValueProvider.ProviderName, await NormalizeProviderKeyAsync(providerKey));
    }

    protected override async Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        if (providerKey == null)
        {
            providerKey =  CurrentTenant.Id?.ToString();
        }

        if (Guid.TryParse(providerKey, out var parsedTenantId))
        {
            var tenant = await TenantRepository.FindByIdAsync(parsedTenantId);
            return tenant?.GetActiveEditionId()?.ToString();
        }

        return null;
    }
}
