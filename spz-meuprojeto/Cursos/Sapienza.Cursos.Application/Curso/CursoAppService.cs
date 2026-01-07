using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.Curso.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Curso;

/// <summary>
/// Application service for Curso entity
/// </summary>
[Authorize(CursoPermissions.Default)]
public class CursoAppService :
    CursosAppService,
    ICursoAppService
{
    private readonly IRepository<Sapienza.Cursos.Curso.Curso, Guid> _repository;

    public CursoAppService(
        IRepository<Sapienza.Cursos.Curso.Curso, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Curso by Id
    /// </summary>
    public virtual async Task<CursoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.Curso.Curso, CursoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Cursos
    /// </summary>
    public virtual async Task<PagedResultDto<CursoDto>> GetListAsync(CursoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.Curso.Curso>, List<CursoDto>>(entities);

        return new PagedResultDto<CursoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Curso
    /// </summary>
    [Authorize(CursoPermissions.Create)]
    public virtual async Task<CursoDto> CreateAsync(CreateUpdateCursoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCursoDto, Sapienza.Cursos.Curso.Curso>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Curso.Curso, CursoDto>(entity);
    }

    /// <summary>
    /// Updates an existing Curso
    /// </summary>
    [Authorize(CursoPermissions.Update)]
    public virtual async Task<CursoDto> UpdateAsync(Guid id, CreateUpdateCursoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.Curso.Curso), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Curso.Curso, CursoDto>(entity);
    }

    /// <summary>
    /// Deletes a Curso
    /// </summary>
    [Authorize(CursoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCursoLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Cursos.Curso.Curso> ApplyFilters(IQueryable<Sapienza.Cursos.Curso.Curso> queryable, CursoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            // ========== FK Filters ==========
            ;
    }
}
