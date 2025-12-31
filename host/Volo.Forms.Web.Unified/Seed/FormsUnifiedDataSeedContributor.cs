using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Forms.Seed;

public class FormsUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly FormsSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly FormsSampleDataSeeder _formsSampleDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public FormsUnifiedDataSeedContributor(
        FormsSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager,
        FormsSampleDataSeeder formsSampleDataSeeder)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
        _formsSampleDataSeeder = formsSampleDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current.SaveChangesAsync();
        await _sampleIdentityDataSeeder.SeedAsync(context);
        await _unitOfWorkManager.Current.SaveChangesAsync();
        await _formsSampleDataSeeder.SeedAsync(context);
    }
}
