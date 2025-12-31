using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Pro.Seed;

public class AbpOpenIddictProUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly AbpOpenIddictProSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly AbpOpenIddictProSampleDataSeeder _proSampleDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICurrentTenant _currentTenant;

    public AbpOpenIddictProUnifiedDataSeedContributor(
        AbpOpenIddictProSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager,
        AbpOpenIddictProSampleDataSeeder proSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
        _proSampleDataSeeder = proSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current.SaveChangesAsync();

        using (_currentTenant.Change(context?.TenantId))
        {
            await _sampleIdentityDataSeeder.SeedAsync(context);
            await _unitOfWorkManager.Current.SaveChangesAsync();
            await _proSampleDataSeeder.SeedAsync(context);
        }
    }
}
