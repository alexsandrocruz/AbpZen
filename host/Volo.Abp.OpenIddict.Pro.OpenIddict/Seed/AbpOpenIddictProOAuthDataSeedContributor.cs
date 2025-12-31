using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict.Pro.Seed;

public class AbpOpenIddictProOAuthDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly AbpOpenIddictProSampleIdentityDataSeeder _abpOpenIddictProSampleIdentityDataSeeder;
    private readonly AbpOpenIddictProOAuthDataSeeder _abpOpenIddictProOAuthDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public AbpOpenIddictProOAuthDataSeedContributor(
        AbpOpenIddictProOAuthDataSeeder abpOpenIddictProOAuthDataSeeder,
        AbpOpenIddictProSampleIdentityDataSeeder abpOpenIddictProSampleIdentityDataSeeder,
        ICurrentTenant currentTenant)
    {
        _abpOpenIddictProOAuthDataSeeder = abpOpenIddictProOAuthDataSeeder;
        _abpOpenIddictProSampleIdentityDataSeeder = abpOpenIddictProSampleIdentityDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _abpOpenIddictProSampleIdentityDataSeeder.SeedAsync(context);
            await _abpOpenIddictProOAuthDataSeeder.SeedAsync(context);
        }
    }
}
