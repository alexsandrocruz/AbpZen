using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Forms.Seed;

/* Creates a few more sample users to test the application and the module.
 *
 * This class is shared among these projects:
 * - Volo.Forms.IdentityServer
 * - Volo.Forms.Common.Web.Unified (used as linked file)
 */
public class FormsSampleIdentityDataSeeder : ITransientDependency
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IOptions<IdentityOptions> _identityOptions;
    private readonly ICurrentTenant _currentTenant;

    public FormsSampleIdentityDataSeeder(
        IIdentityUserRepository identityUserRepository,
        ILookupNormalizer lookupNormalizer,
        IGuidGenerator guidGenerator,
        IdentityUserManager identityUserManager,
        IOptions<IdentityOptions> identityOptions, ICurrentTenant currentTenant)
    {
        _identityUserRepository = identityUserRepository;
        _lookupNormalizer = lookupNormalizer;
        _guidGenerator = guidGenerator;
        _identityUserManager = identityUserManager;
        _identityOptions = identityOptions;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(null))
        {
            if (await _identityUserRepository.GetCountAsync() == 0)
            {
                await CreateUserAsync("john", "John", "Nash", context);
                await CreateUserAsync("albert", "Albert", "Einstein", context);
            }
        }
    }

    private async Task CreateUserAsync(string userName, string name, string surname, DataSeedContext context)
    {
        await _identityOptions.SetAsync();

        if ((await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName(userName))) !=
            null)
        {
            return;
        }

        var user = new IdentityUser(
            _guidGenerator.Create(),
            userName,
            userName + "@abp.io",
            context.TenantId
        );

        user.Name = name;
        user.Surname = surname;

        await _identityUserManager.CreateAsync(user,
            "1q2w3E*"
        );

        await _identityUserManager.AddToRoleAsync(user, "admin");
    }
}