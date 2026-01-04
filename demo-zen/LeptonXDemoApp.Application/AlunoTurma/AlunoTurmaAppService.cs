using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.AlunoTurma.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.AlunoTurma;

/// <summary>
/// Application service for AlunoTurma entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.AlunoTurma.Default)]
public class AlunoTurmaAppService :
    LeptonXDemoAppAppService,
    IAlunoTurmaAppService
{
    private readonly IRepository<LeptonXDemoApp.AlunoTurma.AlunoTurma, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.Aluno.Aluno, Guid> _alunoRepository;
    private readonly IRepository<LeptonXDemoApp.Turma.Turma, Guid> _turmaRepository;

    public AlunoTurmaAppService(
        IRepository<LeptonXDemoApp.AlunoTurma.AlunoTurma, Guid> repository,
        IRepository<LeptonXDemoApp.Aluno.Aluno, Guid> alunoRepository,
        IRepository<LeptonXDemoApp.Turma.Turma, Guid> turmaRepository
    )
    {
        _repository = repository;
        _alunoRepository = alunoRepository;
        _turmaRepository = turmaRepository;
    }

    /// <summary>
    /// Gets a single AlunoTurma by Id
    /// </summary>
    public virtual async Task<AlunoTurmaDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.AlunoTurma.AlunoTurma, AlunoTurmaDto>(entity);
        var aluno = await _alunoRepository.FindAsync(entity.AlunoId);
        dto.AlunoDisplayName = aluno?.Nome;
        var turma = await _turmaRepository.FindAsync(entity.TurmaId);
        dto.TurmaDisplayName = turma?.Nome;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of AlunoTurmas
    /// </summary>
    public virtual async Task<PagedResultDto<AlunoTurmaDto>> GetListAsync(AlunoTurmaGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.AlunoTurma.AlunoTurma>, List<AlunoTurmaDto>>(entities);
        var alunoIds = entities
            .Select(x => x.AlunoId)
            .Distinct()
            .ToList();

        if (alunoIds.Any())
        {
            var parents = await _alunoRepository.GetListAsync(x => alunoIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Nome);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.AlunoId, out var displayName))
                {
                    dto.AlunoDisplayName = displayName;
                }
            }
        }
        var turmaIds = entities
            .Select(x => x.TurmaId)
            .Distinct()
            .ToList();

        if (turmaIds.Any())
        {
            var parents = await _turmaRepository.GetListAsync(x => turmaIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Nome);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.TurmaId, out var displayName))
                {
                    dto.TurmaDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<AlunoTurmaDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new AlunoTurma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.AlunoTurma.Create)]
    public virtual async Task<AlunoTurmaDto> CreateAsync(CreateUpdateAlunoTurmaDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateAlunoTurmaDto, LeptonXDemoApp.AlunoTurma.AlunoTurma>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.AlunoTurma.AlunoTurma, AlunoTurmaDto>(entity);
    }

    /// <summary>
    /// Updates an existing AlunoTurma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.AlunoTurma.Update)]
    public virtual async Task<AlunoTurmaDto> UpdateAsync(Guid id, CreateUpdateAlunoTurmaDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.AlunoTurma.AlunoTurma), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.AlunoTurma.AlunoTurma, AlunoTurmaDto>(entity);
    }

    /// <summary>
    /// Deletes a AlunoTurma
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.AlunoTurma.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetAlunoTurmaLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Situacao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<LeptonXDemoApp.AlunoTurma.AlunoTurma> ApplyFilters(IQueryable<LeptonXDemoApp.AlunoTurma.AlunoTurma> queryable, AlunoTurmaGetListInput input)
    {
        return queryable
            .WhereIf(input.AlunoId != null, x => x.AlunoId == input.AlunoId)
            .WhereIf(input.TurmaId != null, x => x.TurmaId == input.TurmaId)
            .WhereIf(input.DataMatricula != null, x => x.DataMatricula == input.DataMatricula)
            // ========== FK Filters ==========
            .WhereIf(input.AlunoId != null, x => x.AlunoId == input.AlunoId)
            .WhereIf(input.TurmaId != null, x => x.TurmaId == input.TurmaId)
            ;
    }
}
