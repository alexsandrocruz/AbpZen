using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.CmsKit.Pro.Seed;

public class ProUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ProSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly ProSampleDataSeeder _proSampleDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ProUnifiedDataSeedContributor(
        ProSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager,
        ProSampleDataSeeder proSampleDataSeeder)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
        _proSampleDataSeeder = proSampleDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current.SaveChangesAsync();
        await _sampleIdentityDataSeeder.SeedAsync(context);
        await _unitOfWorkManager.Current.SaveChangesAsync();
        await _proSampleDataSeeder.SeedAsync(context);
    }
}
