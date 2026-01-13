using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreCheckListsGrupos;

/// <summary>
/// Application service for advPreCheckListsGrupos entity
/// </summary>
[Authorize(advPreCheckListsGruposPermissions.Default)]
public class advPreCheckListsGruposAppService :
    LexusAppService,
    IadvPreCheckListsGruposAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, Guid> _repository;

    public advPreCheckListsGruposAppService(
        IRepository<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreCheckListsGrupos by Id
    /// </summary>
    public virtual async Task<advPreCheckListsGruposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, advPreCheckListsGruposDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreCheckListsGruposes
    /// </summary>
    public virtual async Task<PagedResultDto<advPreCheckListsGruposDto>> GetListAsync(advPreCheckListsGruposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos>, List<advPreCheckListsGruposDto>>(entities);

        return new PagedResultDto<advPreCheckListsGruposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreCheckListsGrupos
    /// </summary>
    [Authorize(advPreCheckListsGruposPermissions.Create)]
    public virtual async Task<advPreCheckListsGruposDto> CreateAsync(CreateUpdateadvPreCheckListsGruposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreCheckListsGruposDto, Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, advPreCheckListsGruposDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreCheckListsGrupos
    /// </summary>
    [Authorize(advPreCheckListsGruposPermissions.Update)]
    public virtual async Task<advPreCheckListsGruposDto> UpdateAsync(Guid id, CreateUpdateadvPreCheckListsGruposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, advPreCheckListsGruposDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreCheckListsGrupos
    /// </summary>
    [Authorize(advPreCheckListsGruposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreCheckListsGruposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos> ApplyFilters(IQueryable<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos> queryable, advPreCheckListsGruposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
