using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Turma.Dtos;
using LeptonXDemoApp.AlunoTurma.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Turma;

/// <summary>
/// Application service for Turma entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Turma.Default)]
public class TurmaAppService :
    LeptonXDemoAppAppService,
    ITurmaAppService
{
    private readonly IRepository<LeptonXDemoApp.Turma.Turma, Guid> _repository;

    public TurmaAppService(
        IRepository<LeptonXDemoApp.Turma.Turma, Guid> repository
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
        var dto = ObjectMapper.Map<LeptonXDemoApp.Turma.Turma, TurmaDto>(entity);

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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Turma.Turma>, List<TurmaDto>>(entities);

        return new PagedResultDto<TurmaDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Turma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Turma.Create)]
    public virtual async Task<TurmaDto> CreateAsync(CreateUpdateTurmaDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTurmaDto, LeptonXDemoApp.Turma.Turma>(input);
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

        return ObjectMapper.Map<LeptonXDemoApp.Turma.Turma, TurmaDto>(entity);
    }

    /// <summary>
    /// Updates an existing Turma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Turma.Update)]
    public virtual async Task<TurmaDto> UpdateAsync(Guid id, CreateUpdateTurmaDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.AlunoTurmas);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Turma.Turma), id);
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

        return ObjectMapper.Map<LeptonXDemoApp.Turma.Turma, TurmaDto>(entity);
    }

    /// <summary>
    /// Deletes a Turma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Turma.Delete)]
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
    protected virtual IQueryable<LeptonXDemoApp.Turma.Turma> ApplyFilters(IQueryable<LeptonXDemoApp.Turma.Turma> queryable, TurmaGetListInput input)
    {
        return queryable
            .WhereIf(!input.Nome.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Nome))
            .WhereIf(!input.Codigo.IsNullOrWhiteSpace(), x => x.Codigo.Contains(input.Codigo))
            .WhereIf(input.AnoLetivo != null, x => x.AnoLetivo == input.AnoLetivo)
            // ========== FK Filters ==========
            ;
    }
}
