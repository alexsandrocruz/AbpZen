using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.flwAcoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwAcoes;

/// <summary>
/// Application service for flwAcoes entity
/// </summary>
[Authorize(flwAcoesPermissions.Default)]
public class flwAcoesAppService :
    LexusAppService,
    IflwAcoesAppService
{
    private readonly IRepository<Sapienza.Lexus.flwAcoes.flwAcoes, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> _flwFollowsRepository;

    public flwAcoesAppService(
        IRepository<Sapienza.Lexus.flwAcoes.flwAcoes, Guid> repository,
        IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> flwFollowsRepository
    )
    {
        _repository = repository;
        _flwFollowsRepository = flwFollowsRepository;
    }

    /// <summary>
    /// Gets a single flwAcoes by Id
    /// </summary>
    public virtual async Task<flwAcoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.flwAcoes.flwAcoes, flwAcoesDto>(entity);
        if (entity.flwFollowsId != null)
        {
            var parent = await _flwFollowsRepository.FindAsync(entity.flwFollowsId.Value);
            dto.flwFollowsDisplayName = parent?.data;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of flwAcoeses
    /// </summary>
    public virtual async Task<PagedResultDto<flwAcoesDto>> GetListAsync(flwAcoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.flwAcoes.flwAcoes>, List<flwAcoesDto>>(entities);
        var flwFollowsIds = entities
            .Where(x => x.flwFollowsId != null)
            .Select(x => x.flwFollowsId.Value)
            .Distinct()
            .ToList();

        if (flwFollowsIds.Any())
        {
            var parents = await _flwFollowsRepository.GetListAsync(x => flwFollowsIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.data);

            foreach (var dto in dtoList.Where(x => x.flwFollowsId != null))
            {
                if (parentMap.TryGetValue(dto.flwFollowsId.Value, out var displayName))
                {
                    dto.flwFollowsDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<flwAcoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new flwAcoes
    /// </summary>
    [Authorize(flwAcoesPermissions.Create)]
    public virtual async Task<flwAcoesDto> CreateAsync(CreateUpdateflwAcoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateflwAcoesDto, Sapienza.Lexus.flwAcoes.flwAcoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwAcoes.flwAcoes, flwAcoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing flwAcoes
    /// </summary>
    [Authorize(flwAcoesPermissions.Update)]
    public virtual async Task<flwAcoesDto> UpdateAsync(Guid id, CreateUpdateflwAcoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.flwAcoes.flwAcoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwAcoes.flwAcoes, flwAcoesDto>(entity);
    }

    /// <summary>
    /// Deletes a flwAcoes
    /// </summary>
    [Authorize(flwAcoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetflwAcoesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.flwAcoes.flwAcoes> ApplyFilters(IQueryable<Sapienza.Lexus.flwAcoes.flwAcoes> queryable, flwAcoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idAcao != null, x => x.idAcao == input.idAcao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.diasReagendamento != null, x => x.diasReagendamento == input.diasReagendamento)
            // ========== FK Filters ==========
            .WhereIf(input.flwFollowsId != null, x => x.flwFollowsId == input.flwFollowsId)
            ;
    }
}
