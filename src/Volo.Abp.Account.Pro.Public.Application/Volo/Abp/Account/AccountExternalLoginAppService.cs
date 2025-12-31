using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Account.ExternalLogins;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Volo.Abp.Account;

[Authorize]
public class AccountExternalLoginAppService : ApplicationService, IAccountExternalLoginAppService
{
    protected IdentityUserManager UserManager { get; }
    protected ExternalLoginPasswordVerifiedHelper ExternalLoginPasswordVerifiedHelper { get; }

    public AccountExternalLoginAppService(
        IdentityUserManager userManager,
        ExternalLoginPasswordVerifiedHelper externalLoginPasswordVerifiedHelper)
    {
        UserManager = userManager;
        ExternalLoginPasswordVerifiedHelper = externalLoginPasswordVerifiedHelper;
    }

    public virtual async Task<List<AccountExternalLoginDto>> GetListAsync()
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var externalLogins = await UserManager.GetLoginsAsync(user);
        return externalLogins.Select(x => new AccountExternalLoginDto
        {
            LoginProvider = x.LoginProvider,
            ProviderKey = x.ProviderKey,
            ProviderDisplayName = x.ProviderDisplayName
        }).ToList();
    }

    public virtual async Task DeleteAsync(string loginProvider, string providerKey)
    {
        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
        await UserManager.RemoveLoginAsync(user, loginProvider, providerKey);
        await ExternalLoginPasswordVerifiedHelper.RemovePasswordVerifiedAsync(CurrentUser.GetId(), loginProvider, providerKey);
    }

    [AllowAnonymous]
    public virtual async Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        return await ExternalLoginPasswordVerifiedHelper.HasPasswordVerifiedAsync(userId, loginProvider, providerKey);
    }

    public virtual async Task SetPasswordVerifiedAsync(string loginProvider, string providerKey)
    {
        await ExternalLoginPasswordVerifiedHelper.SetPasswordVerifiedAsync(CurrentUser.GetId(), loginProvider, providerKey);
    }

    public virtual async Task RemovePasswordVerifiedAsync(string loginProvider, string providerKey)
    {
        await ExternalLoginPasswordVerifiedHelper.RemovePasswordVerifiedAsync(CurrentUser.GetId(), loginProvider, providerKey);
    }
}
