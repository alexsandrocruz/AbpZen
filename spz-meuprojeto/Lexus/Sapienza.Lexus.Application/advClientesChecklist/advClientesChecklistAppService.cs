using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesChecklist.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesChecklist;

/// <summary>
/// Application service for advClientesChecklist entity
/// </summary>
[Authorize(advClientesChecklistPermissions.Default)]
public class advClientesChecklistAppService :
    LexusAppService,
    IadvClientesChecklistAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, Guid> _repository;

    public advClientesChecklistAppService(
        IRepository<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesChecklist by Id
    /// </summary>
    public virtual async Task<advClientesChecklistDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, advClientesChecklistDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesChecklists
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesChecklistDto>> GetListAsync(advClientesChecklistGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesChecklist.advClientesChecklist>, List<advClientesChecklistDto>>(entities);

        return new PagedResultDto<advClientesChecklistDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesChecklist
    /// </summary>
    [Authorize(advClientesChecklistPermissions.Create)]
    public virtual async Task<advClientesChecklistDto> CreateAsync(CreateUpdateadvClientesChecklistDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesChecklistDto, Sapienza.Lexus.advClientesChecklist.advClientesChecklist>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, advClientesChecklistDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesChecklist
    /// </summary>
    [Authorize(advClientesChecklistPermissions.Update)]
    public virtual async Task<advClientesChecklistDto> UpdateAsync(Guid id, CreateUpdateadvClientesChecklistDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesChecklist.advClientesChecklist), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, advClientesChecklistDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesChecklist
    /// </summary>
    [Authorize(advClientesChecklistPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesChecklistLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesChecklist.advClientesChecklist> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesChecklist.advClientesChecklist> queryable, advClientesChecklistGetListInput input)
    {
        return queryable
            .WhereIf(input.idClienteChecklist != null, x => x.idClienteChecklist == input.idClienteChecklist)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idTipoArquivo != null, x => x.idTipoArquivo == input.idTipoArquivo)
            // ========== FK Filters ==========
            ;
    }
}
