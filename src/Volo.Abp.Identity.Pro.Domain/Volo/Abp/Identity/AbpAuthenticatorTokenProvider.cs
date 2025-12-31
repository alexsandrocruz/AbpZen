using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity;

public class AbpAuthenticatorTokenProvider : AuthenticatorTokenProvider<IdentityUser>, ITransientDependency
{
    public async override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        return user.HasAuthenticator() && await base.CanGenerateTwoFactorTokenAsync(manager, user);
    }
}
