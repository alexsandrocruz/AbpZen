using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Scopes;

public interface IScopeAppService : ICrudAppService<ScopeDto, Guid, GetScopeListInput, CreateScopeInput, UpdateScopeInput>
{
    Task<List<ScopeDto>> GetAllScopesAsync();
}
