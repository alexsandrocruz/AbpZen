using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabEstados.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabEstados;

/// <summary>
/// Application service for fabEstados entity
/// </summary>
[Authorize(fabEstadosPermissions.Default)]
public class fabEstadosAppService :
    LexusAppService,
    IfabEstadosAppService
{
    private readonly IRepository<Sapienza.Lexus.fabEstados.fabEstados, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.fabCidades.fabCidades, Guid> _fabCidadesRepository;

    public fabEstadosAppService(
        IRepository<Sapienza.Lexus.fabEstados.fabEstados, Guid> repository,
        IRepository<Sapienza.Lexus.fabCidades.fabCidades, Guid> fabCidadesRepository
    )
    {
        _repository = repository;
        _fabCidadesRepository = fabCidadesRepository;
    }

    /// <summary>
    /// Gets a single fabEstados by Id
    /// </summary>
    public virtual async Task<fabEstadosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabEstados.fabEstados, fabEstadosDto>(entity);
        if (entity.fabCidadesId != null)
        {
            var parent = await _fabCidadesRepository.FindAsync(entity.fabCidadesId.Value);
            dto.fabCidadesDisplayName = parent?.descricao;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabEstadoses
    /// </summary>
    public virtual async Task<PagedResultDto<fabEstadosDto>> GetListAsync(fabEstadosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabEstados.fabEstados>, List<fabEstadosDto>>(entities);
        var fabCidadesIds = entities
            .Where(x => x.fabCidadesId != null)
            .Select(x => x.fabCidadesId.Value)
            .Distinct()
            .ToList();

        if (fabCidadesIds.Any())
        {
            var parents = await _fabCidadesRepository.GetListAsync(x => fabCidadesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.descricao);

            foreach (var dto in dtoList.Where(x => x.fabCidadesId != null))
            {
                if (parentMap.TryGetValue(dto.fabCidadesId.Value, out var displayName))
                {
                    dto.fabCidadesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<fabEstadosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabEstados
    /// </summary>
    [Authorize(fabEstadosPermissions.Create)]
    public virtual async Task<fabEstadosDto> CreateAsync(CreateUpdatefabEstadosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabEstadosDto, Sapienza.Lexus.fabEstados.fabEstados>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabEstados.fabEstados, fabEstadosDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabEstados
    /// </summary>
    [Authorize(fabEstadosPermissions.Update)]
    public virtual async Task<fabEstadosDto> UpdateAsync(Guid id, CreateUpdatefabEstadosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabEstados.fabEstados), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabEstados.fabEstados, fabEstadosDto>(entity);
    }

    /// <summary>
    /// Deletes a fabEstados
    /// </summary>
    [Authorize(fabEstadosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabEstadosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.sigla
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabEstados.fabEstados> ApplyFilters(IQueryable<Sapienza.Lexus.fabEstados.fabEstados> queryable, fabEstadosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.sigla.Contains(input.Filter) || x.descricao.Contains(input.Filter))
            .WhereIf(input.idEstado != null, x => x.idEstado == input.idEstado)
            .WhereIf(!input.sigla.IsNullOrWhiteSpace(), x => x.sigla.Contains(input.sigla))
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(input.idPais != null, x => x.idPais == input.idPais)
            // ========== FK Filters ==========
            .WhereIf(input.fabCidadesId != null, x => x.fabCidadesId == input.fabCidadesId)
            ;
    }
}
