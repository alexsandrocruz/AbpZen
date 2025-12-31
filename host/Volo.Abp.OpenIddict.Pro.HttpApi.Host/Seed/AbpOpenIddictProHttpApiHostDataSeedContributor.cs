using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict.Pro.Seed;

public class AbpOpenIddictProHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly AbpOpenIddictProSampleDataSeeder _abpOpenIddictProSampleDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public AbpOpenIddictProHttpApiHostDataSeedContributor(
        AbpOpenIddictProSampleDataSeeder abpOpenIddictProSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _abpOpenIddictProSampleDataSeeder = abpOpenIddictProSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _abpOpenIddictProSampleDataSeeder.SeedAsync(context);
        }
    }
}
