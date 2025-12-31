using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Localization;

namespace Volo.Abp.Identity;

public abstract class IdentityAppServiceBase : ApplicationService
{
    protected IdentityAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        LocalizationResource = typeof(IdentityResource);
    }

    protected async Task<List<IdentityUserDto>> FillIdentityUserDtoAsync(IIdentityUserRepository identityUserRepository, IdentityProTwoFactorManager identityProTwoFactorManager, List<IdentityUser> users)
    {
        var userRoles = await identityUserRepository.GetRoleNamesAsync(users.Select(x => x.Id));
        var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users);

        var twoFactorEnabled = await identityProTwoFactorManager.IsOptionalAsync();
        for (var i = 0; i < users.Count; i++)
        {
            userDtos[i].IsLockedOut = users[i].LockoutEnabled && (users[i].LockoutEnd != null && users[i].LockoutEnd > DateTime.UtcNow);
            if (!userDtos[i].IsLockedOut)
            {
                userDtos[i].LockoutEnd = null;
            }
            userDtos[i].SupportTwoFactor = twoFactorEnabled;
            var userRole = userRoles.FirstOrDefault(x => x.Id == users[i].Id);
            userDtos[i].RoleNames = userRole != null ? userRole.RoleNames.ToList() : new List<string>();
        }

        return userDtos;
    }
}
