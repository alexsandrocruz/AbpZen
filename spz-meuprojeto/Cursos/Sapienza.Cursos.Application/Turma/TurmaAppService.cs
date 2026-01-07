using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.Turma.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Turma;

/// <summary>
/// Application service for Turma entity
/// </summary>
[Authorize(Sapienza.CursosPermissions.Turma.Default)]
public class TurmaAppService :
    Sapienza.CursosAppService,
    ITurmaAppService
{
    private readonly IRepository<Sapienza.Cursos.Turma.Turma, Guid> _repository;

    public TurmaAppService(
        IRepository<Sapienza.Cursos.Turma.Turma, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Turma by Id
    /// </summary>
    public virtual async Task<TurmaDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.Turma.Turma, TurmaDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Turmas
    /// </summary>
    public virtual async Task<PagedResultDto<TurmaDto>> GetListAsync(TurmaGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.Turma.Turma>, List<TurmaDto>>(entities);

        return new PagedResultDto<TurmaDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Turma
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.Turma.Create)]
    public virtual async Task<TurmaDto> CreateAsync(CreateUpdateTurmaDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTurmaDto, Sapienza.Cursos.Turma.Turma>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Turma.Turma, TurmaDto>(entity);
    }

    /// <summary>
    /// Updates an existing Turma
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.Turma.Update)]
    public virtual async Task<TurmaDto> UpdateAsync(Guid id, CreateUpdateTurmaDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.Turma.Turma), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Turma.Turma, TurmaDto>(entity);
    }

    /// <summary>
    /// Deletes a Turma
    /// </summary>
    [Authorize(Sapienza.CursosPermissions.Turma.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTurmaLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Nome
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Cursos.Turma.Turma> ApplyFilters(IQueryable<Sapienza.Cursos.Turma.Turma> queryable, TurmaGetListInput input)
    {
        return queryable
            .WhereIf(!input.Nome.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Nome))
            // ========== FK Filters ==========
            ;
    }
}
