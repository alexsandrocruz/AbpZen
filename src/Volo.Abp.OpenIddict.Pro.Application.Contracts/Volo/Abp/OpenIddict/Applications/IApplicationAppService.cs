using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Applications.Dtos;

namespace Volo.Abp.OpenIddict.Applications;

public interface IApplicationAppService : ICrudAppService<ApplicationDto, Guid, GetApplicationListInput, CreateApplicationInput, UpdateApplicationInput>
{
    Task<ApplicationTokenLifetimeDto> GetTokenLifetimeAsync(Guid id);

    Task SetTokenLifetimeAsync(Guid id, ApplicationTokenLifetimeDto input);
}
