using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabPaises.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabPaises;

/// <summary>
/// Application service for fabPaises entity
/// </summary>
[Authorize(fabPaisesPermissions.Default)]
public class fabPaisesAppService :
    LexusAppService,
    IfabPaisesAppService
{
    private readonly IRepository<Sapienza.Lexus.fabPaises.fabPaises, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.fabEstados.fabEstados, Guid> _fabEstadosRepository;

    public fabPaisesAppService(
        IRepository<Sapienza.Lexus.fabPaises.fabPaises, Guid> repository,
        IRepository<Sapienza.Lexus.fabEstados.fabEstados, Guid> fabEstadosRepository
    )
    {
        _repository = repository;
        _fabEstadosRepository = fabEstadosRepository;
    }

    /// <summary>
    /// Gets a single fabPaises by Id
    /// </summary>
    public virtual async Task<fabPaisesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabPaises.fabPaises, fabPaisesDto>(entity);
        if (entity.fabEstadosId != null)
        {
            var parent = await _fabEstadosRepository.FindAsync(entity.fabEstadosId.Value);
            dto.fabEstadosDisplayName = parent?.sigla;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabPaiseses
    /// </summary>
    public virtual async Task<PagedResultDto<fabPaisesDto>> GetListAsync(fabPaisesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabPaises.fabPaises>, List<fabPaisesDto>>(entities);
        var fabEstadosIds = entities
            .Where(x => x.fabEstadosId != null)
            .Select(x => x.fabEstadosId.Value)
            .Distinct()
            .ToList();

        if (fabEstadosIds.Any())
        {
            var parents = await _fabEstadosRepository.GetListAsync(x => fabEstadosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.sigla);

            foreach (var dto in dtoList.Where(x => x.fabEstadosId != null))
            {
                if (parentMap.TryGetValue(dto.fabEstadosId.Value, out var displayName))
                {
                    dto.fabEstadosDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<fabPaisesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabPaises
    /// </summary>
    [Authorize(fabPaisesPermissions.Create)]
    public virtual async Task<fabPaisesDto> CreateAsync(CreateUpdatefabPaisesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabPaisesDto, Sapienza.Lexus.fabPaises.fabPaises>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPaises.fabPaises, fabPaisesDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabPaises
    /// </summary>
    [Authorize(fabPaisesPermissions.Update)]
    public virtual async Task<fabPaisesDto> UpdateAsync(Guid id, CreateUpdatefabPaisesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabPaises.fabPaises), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPaises.fabPaises, fabPaisesDto>(entity);
    }

    /// <summary>
    /// Deletes a fabPaises
    /// </summary>
    [Authorize(fabPaisesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabPaisesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabPaises.fabPaises> ApplyFilters(IQueryable<Sapienza.Lexus.fabPaises.fabPaises> queryable, fabPaisesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idPais != null, x => x.idPais == input.idPais)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.fabEstadosId != null, x => x.fabEstadosId == input.fabEstadosId)
            ;
    }
}
