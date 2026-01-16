using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabPermissoesTipos;

/// <summary>
/// Application service for fabPermissoesTipos entity
/// </summary>
[Authorize(fabPermissoesTiposPermissions.Default)]
public class fabPermissoesTiposAppService :
    LexusAppService,
    IfabPermissoesTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.fabPermissoes.fabPermissoes, Guid> _fabPermissoesRepository;

    public fabPermissoesTiposAppService(
        IRepository<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, Guid> repository,
        IRepository<Sapienza.Lexus.fabPermissoes.fabPermissoes, Guid> fabPermissoesRepository
    )
    {
        _repository = repository;
        _fabPermissoesRepository = fabPermissoesRepository;
    }

    /// <summary>
    /// Gets a single fabPermissoesTipos by Id
    /// </summary>
    public virtual async Task<fabPermissoesTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, fabPermissoesTiposDto>(entity);
        if (entity.fabPermissoesId != null)
        {
            var parent = await _fabPermissoesRepository.FindAsync(entity.fabPermissoesId.Value);
            dto.fabPermissoesDisplayName = parent?.descricao;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabPermissoesTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<fabPermissoesTiposDto>> GetListAsync(fabPermissoesTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos>, List<fabPermissoesTiposDto>>(entities);
        var fabPermissoesIds = entities
            .Where(x => x.fabPermissoesId != null)
            .Select(x => x.fabPermissoesId.Value)
            .Distinct()
            .ToList();

        if (fabPermissoesIds.Any())
        {
            var parents = await _fabPermissoesRepository.GetListAsync(x => fabPermissoesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.descricao);

            foreach (var dto in dtoList.Where(x => x.fabPermissoesId != null))
            {
                if (parentMap.TryGetValue(dto.fabPermissoesId.Value, out var displayName))
                {
                    dto.fabPermissoesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<fabPermissoesTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabPermissoesTipos
    /// </summary>
    [Authorize(fabPermissoesTiposPermissions.Create)]
    public virtual async Task<fabPermissoesTiposDto> CreateAsync(CreateUpdatefabPermissoesTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabPermissoesTiposDto, Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, fabPermissoesTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabPermissoesTipos
    /// </summary>
    [Authorize(fabPermissoesTiposPermissions.Update)]
    public virtual async Task<fabPermissoesTiposDto> UpdateAsync(Guid id, CreateUpdatefabPermissoesTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, fabPermissoesTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a fabPermissoesTipos
    /// </summary>
    [Authorize(fabPermissoesTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabPermissoesTiposLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.descricao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos> ApplyFilters(IQueryable<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos> queryable, fabPermissoesTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.descricao.Contains(input.Filter))
            .WhereIf(input.idPermissaoTipo != null, x => x.idPermissaoTipo == input.idPermissaoTipo)
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            // ========== FK Filters ==========
            .WhereIf(input.fabPermissoesId != null, x => x.fabPermissoesId == input.fabPermissoesId)
            ;
    }
}
