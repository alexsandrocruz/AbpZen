using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.LanguageManagement.Data;

public class LanguageManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly LanguageManagementDataSeeder _languageManagementDataSeeder;

    public LanguageManagementDataSeedContributor(LanguageManagementDataSeeder languageManagementDataSeeder)
    {
        _languageManagementDataSeeder = languageManagementDataSeeder;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId != null)
        {
            /* Language is not multi-tenant */
            return;
        }

        await _languageManagementDataSeeder.SeedAsync();
    }
}
