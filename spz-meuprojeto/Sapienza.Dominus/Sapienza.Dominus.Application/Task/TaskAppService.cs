using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Task.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Task;

/// <summary>
/// Application service for Task entity
/// </summary>
[Authorize(TaskPermissions.Default)]
public class TaskAppService :
    DominusAppService,
    ITaskAppService
{
    private readonly IRepository<Dominus.Task.Task, Guid> _repository;
    private readonly IRepository<Dominus.Project.Project, Guid> _projectRepository;

    public TaskAppService(
        IRepository<Dominus.Task.Task, Guid> repository,
        IRepository<Dominus.Project.Project, Guid> projectRepository
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Gets a single Task by Id
    /// </summary>
    public virtual async Task<TaskDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Task.Task, TaskDto>(entity);
        if (entity.ProjectId != null)
        {
            var parent = await _projectRepository.FindAsync(entity.ProjectId.Value);
            dto.ProjectDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Tasks
    /// </summary>
    public virtual async Task<PagedResultDto<TaskDto>> GetListAsync(TaskGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Task.Task>, List<TaskDto>>(entities);
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

        return new PagedResultDto<TaskDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Task
    /// </summary>
    [Authorize(TaskPermissions.Create)]
    public virtual async Task<TaskDto> CreateAsync(CreateUpdateTaskDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTaskDto, Dominus.Task.Task>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Task.Task, TaskDto>(entity);
    }

    /// <summary>
    /// Updates an existing Task
    /// </summary>
    [Authorize(TaskPermissions.Update)]
    public virtual async Task<TaskDto> UpdateAsync(Guid id, CreateUpdateTaskDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Task.Task), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Task.Task, TaskDto>(entity);
    }

    /// <summary>
    /// Deletes a Task
    /// </summary>
    [Authorize(TaskPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTaskLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.Task.Task> ApplyFilters(IQueryable<Dominus.Task.Task> queryable, TaskGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProjectId != null, x => x.ProjectId == input.ProjectId)
            ;
    }
}
