using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.TaskComment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TaskComment;

/// <summary>
/// Application service for TaskComment entity
/// </summary>
[Authorize(TaskCommentPermissions.Default)]
public class TaskCommentAppService :
    DominusAppService,
    ITaskCommentAppService
{
    private readonly IRepository<Dominus.TaskComment.TaskComment, Guid> _repository;
    private readonly IRepository<Dominus.Task.Task, Guid> _taskRepository;

    public TaskCommentAppService(
        IRepository<Dominus.TaskComment.TaskComment, Guid> repository,
        IRepository<Dominus.Task.Task, Guid> taskRepository
    )
    {
        _repository = repository;
        _taskRepository = taskRepository;
    }

    /// <summary>
    /// Gets a single TaskComment by Id
    /// </summary>
    public virtual async Task<TaskCommentDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.TaskComment.TaskComment, TaskCommentDto>(entity);
        if (entity.TaskId != null)
        {
            var parent = await _taskRepository.FindAsync(entity.TaskId.Value);
            dto.TaskDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of TaskComments
    /// </summary>
    public virtual async Task<PagedResultDto<TaskCommentDto>> GetListAsync(TaskCommentGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.TaskComment.TaskComment>, List<TaskCommentDto>>(entities);
        var taskIds = entities
            .Where(x => x.TaskId != null)
            .Select(x => x.TaskId.Value)
            .Distinct()
            .ToList();

        if (taskIds.Any())
        {
            var parents = await _taskRepository.GetListAsync(x => taskIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.TaskId != null))
            {
                if (parentMap.TryGetValue(dto.TaskId.Value, out var displayName))
                {
                    dto.TaskDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<TaskCommentDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new TaskComment
    /// </summary>
    [Authorize(TaskCommentPermissions.Create)]
    public virtual async Task<TaskCommentDto> CreateAsync(CreateUpdateTaskCommentDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTaskCommentDto, Dominus.TaskComment.TaskComment>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TaskComment.TaskComment, TaskCommentDto>(entity);
    }

    /// <summary>
    /// Updates an existing TaskComment
    /// </summary>
    [Authorize(TaskCommentPermissions.Update)]
    public virtual async Task<TaskCommentDto> UpdateAsync(Guid id, CreateUpdateTaskCommentDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.TaskComment.TaskComment), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TaskComment.TaskComment, TaskCommentDto>(entity);
    }

    /// <summary>
    /// Deletes a TaskComment
    /// </summary>
    [Authorize(TaskCommentPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTaskCommentLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Content
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.TaskComment.TaskComment> ApplyFilters(IQueryable<Dominus.TaskComment.TaskComment> queryable, TaskCommentGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.TaskId != null, x => x.TaskId == input.TaskId)
            ;
    }
}
