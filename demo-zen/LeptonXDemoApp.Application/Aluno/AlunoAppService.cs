using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Aluno.Dtos;
using LeptonXDemoApp.AlunoTurma.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Aluno;

/// <summary>
/// Application service for Aluno entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Aluno.Default)]
public class AlunoAppService :
    LeptonXDemoAppAppService,
    IAlunoAppService
{
    private readonly IRepository<LeptonXDemoApp.Aluno.Aluno, Guid> _repository;

    public AlunoAppService(
        IRepository<LeptonXDemoApp.Aluno.Aluno, Guid> repository
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
        var dto = ObjectMapper.Map<LeptonXDemoApp.Aluno.Aluno, AlunoDto>(entity);

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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Aluno.Aluno>, List<AlunoDto>>(entities);

        return new PagedResultDto<AlunoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Aluno
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Aluno.Create)]
    public virtual async Task<AlunoDto> CreateAsync(CreateUpdateAlunoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAlunoDto, LeptonXDemoApp.Aluno.Aluno>(input);
        // Master-Detail: AlunoTurma
        if (input.AlunoTurmas != null && input.AlunoTurmas.Any())
        {
            foreach (var itemDto in input.AlunoTurmas)
            {
                var item = ObjectMapper.Map<CreateUpdateAlunoTurmaDto, LeptonXDemoApp.AlunoTurma.AlunoTurma>(itemDto);
                entity.AlunoTurmas.Add(item);
            }
        }

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Aluno.Aluno, AlunoDto>(entity);
    }

    /// <summary>
    /// Updates an existing Aluno
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Aluno.Update)]
    public virtual async Task<AlunoDto> UpdateAsync(Guid id, CreateUpdateAlunoDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.AlunoTurmas);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Aluno.Aluno), id);
        }

        ObjectMapper.Map(input, entity);
        // Master-Detail Reconciliation: AlunoTurma
        if (input.AlunoTurmas != null)
        {
            // 1. Remove deleted items
            var inputIds = input.AlunoTurmas.Select(x => x.Id).Where(x => x != Guid.Empty).ToList();
            var itemsToRemove = entity.AlunoTurmas.Where(x => !inputIds.Contains(x.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                entity.AlunoTurmas.Remove(item);
            }

            // 2. Add or Update
            foreach (var itemDto in input.AlunoTurmas)
            {
                if (itemDto.Id == Guid.Empty)
                {
                    // Add new
                    var newItem = ObjectMapper.Map<CreateUpdateAlunoTurmaDto, LeptonXDemoApp.AlunoTurma.AlunoTurma>(itemDto);
                    entity.AlunoTurmas.Add(newItem);
                }
                else
                {
                    // Update existing
                    var existingItem = entity.AlunoTurmas.FirstOrDefault(x => x.Id == itemDto.Id);
                    if (existingItem != null)
                    {
                        ObjectMapper.Map(itemDto, existingItem);
                    }
                }
            }
        }

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Aluno.Aluno, AlunoDto>(entity);
    }

    /// <summary>
    /// Deletes a Aluno
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Aluno.Delete)]
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
    protected virtual IQueryable<LeptonXDemoApp.Aluno.Aluno> ApplyFilters(IQueryable<LeptonXDemoApp.Aluno.Aluno> queryable, AlunoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Nome.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Nome))
            .WhereIf(!input.Email.IsNullOrWhiteSpace(), x => x.Email.Contains(input.Email))
            .WhereIf(!input.Matricula.IsNullOrWhiteSpace(), x => x.Matricula.Contains(input.Matricula))
            // ========== FK Filters ==========
            ;
    }
}
