using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finAreas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finAreas;

/// <summary>
/// Application service for finAreas entity
/// </summary>
[Authorize(finAreasPermissions.Default)]
public class finAreasAppService :
    LexusAppService,
    IfinAreasAppService
{
    private readonly IRepository<Sapienza.Lexus.finAreas.finAreas, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> _finLancamentosRepository;

    public finAreasAppService(
        IRepository<Sapienza.Lexus.finAreas.finAreas, Guid> repository,
        IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid> finLancamentosRepository
    )
    {
        _repository = repository;
        _finLancamentosRepository = finLancamentosRepository;
    }

    /// <summary>
    /// Gets a single finAreas by Id
    /// </summary>
    public virtual async Task<finAreasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finAreas.finAreas, finAreasDto>(entity);
        if (entity.finLancamentosId != null)
        {
            var parent = await _finLancamentosRepository.FindAsync(entity.finLancamentosId.Value);
            dto.finLancamentosDisplayName = parent?.operacao;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finAreases
    /// </summary>
    public virtual async Task<PagedResultDto<finAreasDto>> GetListAsync(finAreasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finAreas.finAreas>, List<finAreasDto>>(entities);
        var finLancamentosIds = entities
            .Where(x => x.finLancamentosId != null)
            .Select(x => x.finLancamentosId.Value)
            .Distinct()
            .ToList();

        if (finLancamentosIds.Any())
        {
            var parents = await _finLancamentosRepository.GetListAsync(x => finLancamentosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.operacao);

            foreach (var dto in dtoList.Where(x => x.finLancamentosId != null))
            {
                if (parentMap.TryGetValue(dto.finLancamentosId.Value, out var displayName))
                {
                    dto.finLancamentosDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<finAreasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finAreas
    /// </summary>
    [Authorize(finAreasPermissions.Create)]
    public virtual async Task<finAreasDto> CreateAsync(CreateUpdatefinAreasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinAreasDto, Sapienza.Lexus.finAreas.finAreas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finAreas.finAreas, finAreasDto>(entity);
    }

    /// <summary>
    /// Updates an existing finAreas
    /// </summary>
    [Authorize(finAreasPermissions.Update)]
    public virtual async Task<finAreasDto> UpdateAsync(Guid id, CreateUpdatefinAreasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finAreas.finAreas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finAreas.finAreas, finAreasDto>(entity);
    }

    /// <summary>
    /// Deletes a finAreas
    /// </summary>
    [Authorize(finAreasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinAreasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finAreas.finAreas> ApplyFilters(IQueryable<Sapienza.Lexus.finAreas.finAreas> queryable, finAreasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idCentroResultado != null, x => x.idCentroResultado == input.idCentroResultado)
            // ========== FK Filters ==========
            .WhereIf(input.finLancamentosId != null, x => x.finLancamentosId == input.finLancamentosId)
            ;
    }
}
