using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProOrgaos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProOrgaos;

/// <summary>
/// Application service for advProOrgaos entity
/// </summary>
[Authorize(advProOrgaosPermissions.Default)]
public class advProOrgaosAppService :
    LexusAppService,
    IadvProOrgaosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProOrgaos.advProOrgaos, Guid> _repository;

    public advProOrgaosAppService(
        IRepository<Sapienza.Lexus.advProOrgaos.advProOrgaos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProOrgaos by Id
    /// </summary>
    public virtual async Task<advProOrgaosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProOrgaos.advProOrgaos, advProOrgaosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProOrgaoses
    /// </summary>
    public virtual async Task<PagedResultDto<advProOrgaosDto>> GetListAsync(advProOrgaosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProOrgaos.advProOrgaos>, List<advProOrgaosDto>>(entities);

        return new PagedResultDto<advProOrgaosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProOrgaos
    /// </summary>
    [Authorize(advProOrgaosPermissions.Create)]
    public virtual async Task<advProOrgaosDto> CreateAsync(CreateUpdateadvProOrgaosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProOrgaosDto, Sapienza.Lexus.advProOrgaos.advProOrgaos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProOrgaos.advProOrgaos, advProOrgaosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProOrgaos
    /// </summary>
    [Authorize(advProOrgaosPermissions.Update)]
    public virtual async Task<advProOrgaosDto> UpdateAsync(Guid id, CreateUpdateadvProOrgaosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProOrgaos.advProOrgaos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProOrgaos.advProOrgaos, advProOrgaosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProOrgaos
    /// </summary>
    [Authorize(advProOrgaosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProOrgaosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.titulo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProOrgaos.advProOrgaos> ApplyFilters(IQueryable<Sapienza.Lexus.advProOrgaos.advProOrgaos> queryable, advProOrgaosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idOrgao != null, x => x.idOrgao == input.idOrgao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
