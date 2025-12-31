using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Forms.Seed;

public class FormsHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly FormsSampleDataSeeder _formsSampleDataSeeder;

    public FormsHttpApiHostDataSeedContributor(
        FormsSampleDataSeeder formsSampleDataSeeder)
    {
        _formsSampleDataSeeder = formsSampleDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _formsSampleDataSeeder.SeedAsync(context);
    }
}
