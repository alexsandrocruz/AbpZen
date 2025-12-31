using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity;

public interface IIdentitySessionAppService : IApplicationService
{
    Task<PagedResultDto<IdentitySessionDto>> GetListAsync(GetIdentitySessionListInput input);

    Task<IdentitySessionDto> GetAsync(Guid id);

    Task RevokeAsync(Guid id);
}
