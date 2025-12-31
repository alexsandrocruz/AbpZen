using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Account;

public interface IAccountExternalLoginAppService : IApplicationService
{
    Task<List<AccountExternalLoginDto>> GetListAsync();

    Task DeleteAsync(string loginProvider, string providerKey);

    Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey);

    Task SetPasswordVerifiedAsync(string loginProvider, string providerKey);

    Task RemovePasswordVerifiedAsync(string loginProvider, string providerKey);
}
