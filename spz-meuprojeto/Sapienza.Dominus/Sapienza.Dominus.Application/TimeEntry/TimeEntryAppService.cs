using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.TimeEntry.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TimeEntry;

/// <summary>
/// Application service for TimeEntry entity
/// </summary>
[Authorize(TimeEntryPermissions.Default)]
public class TimeEntryAppService :
    DominusAppService,
    ITimeEntryAppService
{
    private readonly IRepository<Dominus.TimeEntry.TimeEntry, Guid> _repository;
    private readonly IRepository<Dominus.Project.Project, Guid> _projectRepository;

    public TimeEntryAppService(
        IRepository<Dominus.TimeEntry.TimeEntry, Guid> repository,
        IRepository<Dominus.Project.Project, Guid> projectRepository
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Gets a single TimeEntry by Id
    /// </summary>
    public virtual async Task<TimeEntryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.TimeEntry.TimeEntry, TimeEntryDto>(entity);
        if (entity.ProjectId != null)
        {
            var parent = await _projectRepository.FindAsync(entity.ProjectId.Value);
            dto.ProjectDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of TimeEntries
    /// </summary>
    public virtual async Task<PagedResultDto<TimeEntryDto>> GetListAsync(TimeEntryGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.TimeEntry.TimeEntry>, List<TimeEntryDto>>(entities);
        var projectIds = entities
            .Where(x => x.ProjectId != null)
            .Select(x => x.ProjectId.Value)
            .Distinct()
            .ToList();

        if (projectIds.Any())
        {
            var parents = await _projectRepository.GetListAsync(x => projectIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.ProjectId != null))
            {
                if (parentMap.TryGetValue(dto.ProjectId.Value, out var displayName))
                {
                    dto.ProjectDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<TimeEntryDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new TimeEntry
    /// </summary>
    [Authorize(TimeEntryPermissions.Create)]
    public virtual async Task<TimeEntryDto> CreateAsync(CreateUpdateTimeEntryDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTimeEntryDto, Dominus.TimeEntry.TimeEntry>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TimeEntry.TimeEntry, TimeEntryDto>(entity);
    }

    /// <summary>
    /// Updates an existing TimeEntry
    /// </summary>
    [Authorize(TimeEntryPermissions.Update)]
    public virtual async Task<TimeEntryDto> UpdateAsync(Guid id, CreateUpdateTimeEntryDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.TimeEntry.TimeEntry), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TimeEntry.TimeEntry, TimeEntryDto>(entity);
    }

    /// <summary>
    /// Deletes a TimeEntry
    /// </summary>
    [Authorize(TimeEntryPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTimeEntryLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Description
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.TimeEntry.TimeEntry> ApplyFilters(IQueryable<Dominus.TimeEntry.TimeEntry> queryable, TimeEntryGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProjectId != null, x => x.ProjectId == input.ProjectId)
            ;
    }
}
