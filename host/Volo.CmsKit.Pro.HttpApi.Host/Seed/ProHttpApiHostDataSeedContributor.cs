using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Pro.Seed;

public class ProHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ProSampleDataSeeder _proSampleDataSeeder;

    public ProHttpApiHostDataSeedContributor(
        ProSampleDataSeeder proSampleDataSeeder)
    {
        _proSampleDataSeeder = proSampleDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _proSampleDataSeeder.SeedAsync(context);
    }
}
