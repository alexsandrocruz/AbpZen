using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Forms.Seed;

public class FormsIdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly FormsSampleIdentityDataSeeder _formsSampleIdentityDataSeeder;
    private readonly FormsIdentityServerDataSeeder _formsIdentityServerDataSeeder;

    public FormsIdentityServerDataSeedContributor(
        FormsIdentityServerDataSeeder formsIdentityServerDataSeeder,
        FormsSampleIdentityDataSeeder formsSampleIdentityDataSeeder)
    {
        _formsIdentityServerDataSeeder = formsIdentityServerDataSeeder;
        _formsSampleIdentityDataSeeder = formsSampleIdentityDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _formsSampleIdentityDataSeeder.SeedAsync(context);
        await _formsIdentityServerDataSeeder.SeedAsync(context);
    }
}
