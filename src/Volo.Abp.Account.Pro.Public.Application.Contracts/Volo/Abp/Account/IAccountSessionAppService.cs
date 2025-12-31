using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Volo.Abp.Account;

public interface IAccountSessionAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetAccountIdentitySessionListInput input);

    Task<IdentitySessionDto> GetAsync(Guid id);

    Task RevokeAsync(Guid id);
}
