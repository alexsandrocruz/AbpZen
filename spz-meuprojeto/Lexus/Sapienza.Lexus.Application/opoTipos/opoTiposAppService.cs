using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.opoTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.opoTipos;

/// <summary>
/// Application service for opoTipos entity
/// </summary>
[Authorize(opoTiposPermissions.Default)]
public class opoTiposAppService :
    LexusAppService,
    IopoTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.opoTipos.opoTipos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> _opoOportunidadesRepository;

    public opoTiposAppService(
        IRepository<Sapienza.Lexus.opoTipos.opoTipos, Guid> repository,
        IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> opoOportunidadesRepository
    )
    {
        _repository = repository;
        _opoOportunidadesRepository = opoOportunidadesRepository;
    }

    /// <summary>
    /// Gets a single opoTipos by Id
    /// </summary>
    public virtual async Task<opoTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.opoTipos.opoTipos, opoTiposDto>(entity);
        if (entity.opoOportunidadesId != null)
        {
            var parent = await _opoOportunidadesRepository.FindAsync(entity.opoOportunidadesId.Value);
            dto.opoOportunidadesDisplayName = parent?.titulo;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of opoTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<opoTiposDto>> GetListAsync(opoTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.opoTipos.opoTipos>, List<opoTiposDto>>(entities);
        var opoOportunidadesIds = entities
            .Where(x => x.opoOportunidadesId != null)
            .Select(x => x.opoOportunidadesId.Value)
            .Distinct()
            .ToList();

        if (opoOportunidadesIds.Any())
        {
            var parents = await _opoOportunidadesRepository.GetListAsync(x => opoOportunidadesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.titulo);

            foreach (var dto in dtoList.Where(x => x.opoOportunidadesId != null))
            {
                if (parentMap.TryGetValue(dto.opoOportunidadesId.Value, out var displayName))
                {
                    dto.opoOportunidadesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<opoTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new opoTipos
    /// </summary>
    [Authorize(opoTiposPermissions.Create)]
    public virtual async Task<opoTiposDto> CreateAsync(CreateUpdateopoTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateopoTiposDto, Sapienza.Lexus.opoTipos.opoTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoTipos.opoTipos, opoTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing opoTipos
    /// </summary>
    [Authorize(opoTiposPermissions.Update)]
    public virtual async Task<opoTiposDto> UpdateAsync(Guid id, CreateUpdateopoTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.opoTipos.opoTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoTipos.opoTipos, opoTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a opoTipos
    /// </summary>
    [Authorize(opoTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetopoTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.opoTipos.opoTipos> ApplyFilters(IQueryable<Sapienza.Lexus.opoTipos.opoTipos> queryable, opoTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.opoOportunidadesId != null, x => x.opoOportunidadesId == input.opoOportunidadesId)
            ;
    }
}
