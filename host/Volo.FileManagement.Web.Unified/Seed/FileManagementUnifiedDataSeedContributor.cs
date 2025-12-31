using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Volo.FileManagement.IdentityServer.Seed;

namespace Volo.FileManagement.Seed;

public class FileManagementUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly FileManagementSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public FileManagementUnifiedDataSeedContributor(
        FileManagementSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current.SaveChangesAsync();
        await _sampleIdentityDataSeeder.SeedAsync(context);
        await _unitOfWorkManager.Current.SaveChangesAsync();
    }
}
