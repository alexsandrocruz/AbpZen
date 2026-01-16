using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesINSSStatus;

/// <summary>
/// Application service for advClientesINSSStatus entity
/// </summary>
[Authorize(advClientesINSSStatusPermissions.Default)]
public class advClientesINSSStatusAppService :
    LexusAppService,
    IadvClientesINSSStatusAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advClientesINSS.advClientesINSS, Guid> _advClientesINSSRepository;

    public advClientesINSSStatusAppService(
        IRepository<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, Guid> repository,
        IRepository<Sapienza.Lexus.advClientesINSS.advClientesINSS, Guid> advClientesINSSRepository
    )
    {
        _repository = repository;
        _advClientesINSSRepository = advClientesINSSRepository;
    }

    /// <summary>
    /// Gets a single advClientesINSSStatus by Id
    /// </summary>
    public virtual async Task<advClientesINSSStatusDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, advClientesINSSStatusDto>(entity);
        var advClientesINSS = await _advClientesINSSRepository.FindAsync(entity.advClientesINSSId);
        dto.advClientesINSSDisplayName = advClientesINSS?.inssData;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesINSSStatuses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesINSSStatusDto>> GetListAsync(advClientesINSSStatusGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus>, List<advClientesINSSStatusDto>>(entities);
        var advClientesINSSIds = entities
            .Select(x => x.advClientesINSSId)
            .Distinct()
            .ToList();

        if (advClientesINSSIds.Any())
        {
            var parents = await _advClientesINSSRepository.GetListAsync(x => advClientesINSSIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.inssData);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.advClientesINSSId, out var displayName))
                {
                    dto.advClientesINSSDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advClientesINSSStatusDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesINSSStatus
    /// </summary>
    [Authorize(advClientesINSSStatusPermissions.Create)]
    public virtual async Task<advClientesINSSStatusDto> CreateAsync(CreateUpdateadvClientesINSSStatusDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesINSSStatusDto, Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, advClientesINSSStatusDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesINSSStatus
    /// </summary>
    [Authorize(advClientesINSSStatusPermissions.Update)]
    public virtual async Task<advClientesINSSStatusDto> UpdateAsync(Guid id, CreateUpdateadvClientesINSSStatusDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, advClientesINSSStatusDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesINSSStatus
    /// </summary>
    [Authorize(advClientesINSSStatusPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesINSSStatusLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus> queryable, advClientesINSSStatusGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.advClientesINSSId != null, x => x.advClientesINSSId == input.advClientesINSSId)
            ;
    }
}
