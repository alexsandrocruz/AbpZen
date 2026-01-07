using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.NewEntity1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.NewEntity1;

/// <summary>
/// Application service for NewEntity1 entity
/// </summary>
[Authorize(Sapienza.CursosPermissions.NewEntity1.Default)]
public class NewEntity1AppService :
    Sapienza.CursosAppService,
    INewEntity1AppService
{
    private readonly IRepository<Sapienza.Cursos.NewEntity1.NewEntity1, Guid> _repository;

    public NewEntity1AppService(
        IRepository<Sapienza.Cursos.NewEntity1.NewEntity1, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single NewEntity1 by Id
    /// </summary>
    public virtual async Task<NewEntity1Dto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.NewEntity1.NewEntity1, NewEntity1Dto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of NewEntity1s
    /// </summary>
    public virtual async Task<PagedResultDto<NewEntity1Dto>> GetListAsync(NewEntity1GetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.NewEntity1.NewEntity1>, List<NewEntity1Dto>>(entities);

        return new PagedResultDto<NewEntity1Dto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new NewEntity1
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.NewEntity1.Create)]
    public virtual async Task<NewEntity1Dto> CreateAsync(CreateUpdateNewEntity1Dto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateNewEntity1Dto, Sapienza.Cursos.NewEntity1.NewEntity1>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.NewEntity1.NewEntity1, NewEntity1Dto>(entity);
    }

    /// <summary>
    /// Updates an existing NewEntity1
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.NewEntity1.Update)]
    public virtual async Task<NewEntity1Dto> UpdateAsync(Guid id, CreateUpdateNewEntity1Dto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.NewEntity1.NewEntity1), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.NewEntity1.NewEntity1, NewEntity1Dto>(entity);
    }

    /// <summary>
    /// Deletes a NewEntity1
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.NewEntity1.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetNewEntity1LookupAsync()
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
    protected virtual IQueryable<Sapienza.Cursos.NewEntity1.NewEntity1> ApplyFilters(IQueryable<Sapienza.Cursos.NewEntity1.NewEntity1> queryable, NewEntity1GetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
