using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.WorkspaceInvite.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceInvite;

/// <summary>
/// Application service for WorkspaceInvite entity
/// </summary>
[Authorize(WorkspaceInvitePermissions.Default)]
public class WorkspaceInviteAppService :
    DominusAppService,
    IWorkspaceInviteAppService
{
    private readonly IRepository<Dominus.WorkspaceInvite.WorkspaceInvite, Guid> _repository;

    public WorkspaceInviteAppService(
        IRepository<Dominus.WorkspaceInvite.WorkspaceInvite, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single WorkspaceInvite by Id
    /// </summary>
    public virtual async Task<WorkspaceInviteDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.WorkspaceInvite.WorkspaceInvite, WorkspaceInviteDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of WorkspaceInvites
    /// </summary>
    public virtual async Task<PagedResultDto<WorkspaceInviteDto>> GetListAsync(WorkspaceInviteGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.WorkspaceInvite.WorkspaceInvite>, List<WorkspaceInviteDto>>(entities);

        return new PagedResultDto<WorkspaceInviteDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new WorkspaceInvite
    /// </summary>
    [Authorize(WorkspaceInvitePermissions.Create)]
    public virtual async Task<WorkspaceInviteDto> CreateAsync(CreateUpdateWorkspaceInviteDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateWorkspaceInviteDto, Dominus.WorkspaceInvite.WorkspaceInvite>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceInvite.WorkspaceInvite, WorkspaceInviteDto>(entity);
    }

    /// <summary>
    /// Updates an existing WorkspaceInvite
    /// </summary>
    [Authorize(WorkspaceInvitePermissions.Update)]
    public virtual async Task<WorkspaceInviteDto> UpdateAsync(Guid id, CreateUpdateWorkspaceInviteDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.WorkspaceInvite.WorkspaceInvite), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.WorkspaceInvite.WorkspaceInvite, WorkspaceInviteDto>(entity);
    }

    /// <summary>
    /// Deletes a WorkspaceInvite
    /// </summary>
    [Authorize(WorkspaceInvitePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWorkspaceInviteLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Email
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.WorkspaceInvite.WorkspaceInvite> ApplyFilters(IQueryable<Dominus.WorkspaceInvite.WorkspaceInvite> queryable, WorkspaceInviteGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
