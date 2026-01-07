using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.Aluno.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Aluno;

/// <summary>
/// Application service for Aluno entity
/// </summary>
[Authorize(CursosPermissions.Aluno.Default)]
public class AlunoAppService :
    CursosAppService,
    IAlunoAppService
{
    private readonly IRepository<Sapienza.Cursos.Aluno.Aluno, Guid> _repository;

    public AlunoAppService(
        IRepository<Sapienza.Cursos.Aluno.Aluno, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Aluno by Id
    /// </summary>
    public virtual async Task<AlunoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.Aluno.Aluno, AlunoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Alunos
    /// </summary>
    public virtual async Task<PagedResultDto<AlunoDto>> GetListAsync(AlunoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.Aluno.Aluno>, List<AlunoDto>>(entities);

        return new PagedResultDto<AlunoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Aluno
    /// </summary>
    [Authorize(CursosPermissions.Aluno.Create)]
    public virtual async Task<AlunoDto> CreateAsync(CreateUpdateAlunoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAlunoDto, Sapienza.Cursos.Aluno.Aluno>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Aluno.Aluno, AlunoDto>(entity);
    }

    /// <summary>
    /// Updates an existing Aluno
    /// </summary>
    [Authorize(CursosPermissions.Aluno.Update)]
    public virtual async Task<AlunoDto> UpdateAsync(Guid id, CreateUpdateAlunoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.Aluno.Aluno), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Aluno.Aluno, AlunoDto>(entity);
    }

    /// <summary>
    /// Deletes a Aluno
    /// </summary>
    [Authorize(CursosPermissions.Aluno.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetAlunoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Cursos.Aluno.Aluno> ApplyFilters(IQueryable<Sapienza.Cursos.Aluno.Aluno> queryable, AlunoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Nome.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Nome))
            // ========== FK Filters ==========
            ;
    }
}
