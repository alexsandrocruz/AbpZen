using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliPrioridades.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliPrioridades;

/// <summary>
/// Application service for advCliPrioridades entity
/// </summary>
[Authorize(advCliPrioridadesPermissions.Default)]
public class advCliPrioridadesAppService :
    LexusAppService,
    IadvCliPrioridadesAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliPrioridades.advCliPrioridades, Guid> _repository;

    public advCliPrioridadesAppService(
        IRepository<Sapienza.Lexus.advCliPrioridades.advCliPrioridades, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliPrioridades by Id
    /// </summary>
    public virtual async Task<advCliPrioridadesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliPrioridades.advCliPrioridades, advCliPrioridadesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliPrioridadeses
    /// </summary>
    public virtual async Task<PagedResultDto<advCliPrioridadesDto>> GetListAsync(advCliPrioridadesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliPrioridades.advCliPrioridades>, List<advCliPrioridadesDto>>(entities);

        return new PagedResultDto<advCliPrioridadesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliPrioridades
    /// </summary>
    [Authorize(advCliPrioridadesPermissions.Create)]
    public virtual async Task<advCliPrioridadesDto> CreateAsync(CreateUpdateadvCliPrioridadesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliPrioridadesDto, Sapienza.Lexus.advCliPrioridades.advCliPrioridades>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliPrioridades.advCliPrioridades, advCliPrioridadesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliPrioridades
    /// </summary>
    [Authorize(advCliPrioridadesPermissions.Update)]
    public virtual async Task<advCliPrioridadesDto> UpdateAsync(Guid id, CreateUpdateadvCliPrioridadesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliPrioridades.advCliPrioridades), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliPrioridades.advCliPrioridades, advCliPrioridadesDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliPrioridades
    /// </summary>
    [Authorize(advCliPrioridadesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliPrioridadesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliPrioridades.advCliPrioridades> ApplyFilters(IQueryable<Sapienza.Lexus.advCliPrioridades.advCliPrioridades> queryable, advCliPrioridadesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.cor.Contains(input.Filter))
            .WhereIf(input.idPrioridade != null, x => x.idPrioridade == input.idPrioridade)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.cor.IsNullOrWhiteSpace(), x => x.cor.Contains(input.cor))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
