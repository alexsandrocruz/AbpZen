using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuAcessos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuAcessos;

/// <summary>
/// Application service for usuAcessos entity
/// </summary>
[Authorize(usuAcessosPermissions.Default)]
public class usuAcessosAppService :
    LexusAppService,
    IusuAcessosAppService
{
    private readonly IRepository<Sapienza.Lexus.usuAcessos.usuAcessos, Guid> _repository;

    public usuAcessosAppService(
        IRepository<Sapienza.Lexus.usuAcessos.usuAcessos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuAcessos by Id
    /// </summary>
    public virtual async Task<usuAcessosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuAcessos.usuAcessos, usuAcessosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuAcessoses
    /// </summary>
    public virtual async Task<PagedResultDto<usuAcessosDto>> GetListAsync(usuAcessosGetListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        // Apply filters
        queryable = ApplyFilters(queryable, input);

        // Apply default sorting (by CreationTime descending)
        queryable = queryable.OrderByDescending(e => e.CreationTime);

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var entities = await AsyncExecuter.ToListAsync(queryable);
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuAcessos.usuAcessos>, List<usuAcessosDto>>(entities);

        return new PagedResultDto<usuAcessosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuAcessos
    /// </summary>
    [Authorize(usuAcessosPermissions.Create)]
    public virtual async Task<usuAcessosDto> CreateAsync(CreateUpdateusuAcessosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuAcessosDto, Sapienza.Lexus.usuAcessos.usuAcessos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuAcessos.usuAcessos, usuAcessosDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuAcessos
    /// </summary>
    [Authorize(usuAcessosPermissions.Update)]
    public virtual async Task<usuAcessosDto> UpdateAsync(Guid id, CreateUpdateusuAcessosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuAcessos.usuAcessos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuAcessos.usuAcessos, usuAcessosDto>(entity);
    }

    /// <summary>
    /// Deletes a usuAcessos
    /// </summary>
    [Authorize(usuAcessosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuAcessosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.ip
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.usuAcessos.usuAcessos> ApplyFilters(IQueryable<Sapienza.Lexus.usuAcessos.usuAcessos> queryable, usuAcessosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.ip.Contains(input.Filter))
            .WhereIf(input.idAcesso != null, x => x.idAcesso == input.idAcesso)
            .WhereIf(input.IdentityUserId != null, x => x.IdentityUserId == input.IdentityUserId)
            .WhereIf(input.data != null, x => x.data == input.data)
            .WhereIf(!input.ip.IsNullOrWhiteSpace(), x => x.ip.Contains(input.ip))
            // ========== FK Filters ==========
            ;
    }
}
