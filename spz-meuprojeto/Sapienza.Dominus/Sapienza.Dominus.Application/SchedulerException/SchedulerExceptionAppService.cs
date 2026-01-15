using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.SchedulerException.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerException;

/// <summary>
/// Application service for SchedulerException entity
/// </summary>
[Authorize(SchedulerExceptionPermissions.Default)]
public class SchedulerExceptionAppService :
    DominusAppService,
    ISchedulerExceptionAppService
{
    private readonly IRepository<Dominus.SchedulerException.SchedulerException, Guid> _repository;
    private readonly IRepository<Dominus.SchedulerType.SchedulerType, Guid> _schedulerTypeRepository;

    public SchedulerExceptionAppService(
        IRepository<Dominus.SchedulerException.SchedulerException, Guid> repository,
        IRepository<Dominus.SchedulerType.SchedulerType, Guid> schedulerTypeRepository
    )
    {
        _repository = repository;
        _schedulerTypeRepository = schedulerTypeRepository;
    }

    /// <summary>
    /// Gets a single SchedulerException by Id
    /// </summary>
    public virtual async Task<SchedulerExceptionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.SchedulerException.SchedulerException, SchedulerExceptionDto>(entity);
        if (entity.SchedulerTypeId != null)
        {
            var parent = await _schedulerTypeRepository.FindAsync(entity.SchedulerTypeId.Value);
            dto.SchedulerTypeDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of SchedulerExceptions
    /// </summary>
    public virtual async Task<PagedResultDto<SchedulerExceptionDto>> GetListAsync(SchedulerExceptionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.SchedulerException.SchedulerException>, List<SchedulerExceptionDto>>(entities);
        var schedulerTypeIds = entities
            .Where(x => x.SchedulerTypeId != null)
            .Select(x => x.SchedulerTypeId.Value)
            .Distinct()
            .ToList();

        if (schedulerTypeIds.Any())
        {
            var parents = await _schedulerTypeRepository.GetListAsync(x => schedulerTypeIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.SchedulerTypeId != null))
            {
                if (parentMap.TryGetValue(dto.SchedulerTypeId.Value, out var displayName))
                {
                    dto.SchedulerTypeDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<SchedulerExceptionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new SchedulerException
    /// </summary>
    [Authorize(SchedulerExceptionPermissions.Create)]
    public virtual async Task<SchedulerExceptionDto> CreateAsync(CreateUpdateSchedulerExceptionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSchedulerExceptionDto, Dominus.SchedulerException.SchedulerException>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerException.SchedulerException, SchedulerExceptionDto>(entity);
    }

    /// <summary>
    /// Updates an existing SchedulerException
    /// </summary>
    [Authorize(SchedulerExceptionPermissions.Update)]
    public virtual async Task<SchedulerExceptionDto> UpdateAsync(Guid id, CreateUpdateSchedulerExceptionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.SchedulerException.SchedulerException), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.SchedulerException.SchedulerException, SchedulerExceptionDto>(entity);
    }

    /// <summary>
    /// Deletes a SchedulerException
    /// </summary>
    [Authorize(SchedulerExceptionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSchedulerExceptionLookupAsync()
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
    protected virtual IQueryable<Dominus.SchedulerException.SchedulerException> ApplyFilters(IQueryable<Dominus.SchedulerException.SchedulerException> queryable, SchedulerExceptionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SchedulerTypeId != null, x => x.SchedulerTypeId == input.SchedulerTypeId)
            ;
    }
}
