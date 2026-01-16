using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advVerTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advVerTipos;

/// <summary>
/// Application service for advVerTipos entity
/// </summary>
[Authorize(advVerTiposPermissions.Default)]
public class advVerTiposAppService :
    LexusAppService,
    IadvVerTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.advVerTipos.advVerTipos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advVerbas.advVerbas, Guid> _advVerbasRepository;

    public advVerTiposAppService(
        IRepository<Sapienza.Lexus.advVerTipos.advVerTipos, Guid> repository,
        IRepository<Sapienza.Lexus.advVerbas.advVerbas, Guid> advVerbasRepository
    )
    {
        _repository = repository;
        _advVerbasRepository = advVerbasRepository;
    }

    /// <summary>
    /// Gets a single advVerTipos by Id
    /// </summary>
    public virtual async Task<advVerTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advVerTipos.advVerTipos, advVerTiposDto>(entity);
        if (entity.advVerbasId != null)
        {
            var parent = await _advVerbasRepository.FindAsync(entity.advVerbasId.Value);
            dto.advVerbasDisplayName = parent?.dataDe;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advVerTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<advVerTiposDto>> GetListAsync(advVerTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advVerTipos.advVerTipos>, List<advVerTiposDto>>(entities);
        var advVerbasIds = entities
            .Where(x => x.advVerbasId != null)
            .Select(x => x.advVerbasId.Value)
            .Distinct()
            .ToList();

        if (advVerbasIds.Any())
        {
            var parents = await _advVerbasRepository.GetListAsync(x => advVerbasIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.dataDe);

            foreach (var dto in dtoList.Where(x => x.advVerbasId != null))
            {
                if (parentMap.TryGetValue(dto.advVerbasId.Value, out var displayName))
                {
                    dto.advVerbasDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advVerTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advVerTipos
    /// </summary>
    [Authorize(advVerTiposPermissions.Create)]
    public virtual async Task<advVerTiposDto> CreateAsync(CreateUpdateadvVerTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvVerTiposDto, Sapienza.Lexus.advVerTipos.advVerTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advVerTipos.advVerTipos, advVerTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing advVerTipos
    /// </summary>
    [Authorize(advVerTiposPermissions.Update)]
    public virtual async Task<advVerTiposDto> UpdateAsync(Guid id, CreateUpdateadvVerTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advVerTipos.advVerTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advVerTipos.advVerTipos, advVerTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a advVerTipos
    /// </summary>
    [Authorize(advVerTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvVerTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advVerTipos.advVerTipos> ApplyFilters(IQueryable<Sapienza.Lexus.advVerTipos.advVerTipos> queryable, advVerTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.advVerbasId != null, x => x.advVerbasId == input.advVerbasId)
            ;
    }
}
