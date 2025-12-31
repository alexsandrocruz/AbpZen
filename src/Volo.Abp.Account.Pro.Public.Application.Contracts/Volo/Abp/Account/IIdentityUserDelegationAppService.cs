using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Account;

public interface IIdentityUserDelegationAppService: IApplicationService
{
    Task<ListResultDto<UserDelegationDto>> GetDelegatedUsersAsync();

    Task<ListResultDto<UserDelegationDto>> GetMyDelegatedUsersAsync();

    Task<ListResultDto<UserDelegationDto>> GetActiveDelegationsAsync();

    Task<ListResultDto<UserLookupDto>> GetUserLookupAsync(GetUserLookupInput input);

    Task DelegateNewUserAsync(DelegateNewUserInput input);

    Task DeleteDelegationAsync(Guid id);
}
