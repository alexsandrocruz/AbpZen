using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProjectFollower.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectFollower;

/// <summary>
/// Application service for ProjectFollower entity
/// </summary>
[Authorize(ProjectFollowerPermissions.Default)]
public class ProjectFollowerAppService :
    DominusAppService,
    IProjectFollowerAppService
{
    private readonly IRepository<Dominus.ProjectFollower.ProjectFollower, Guid> _repository;
    private readonly IRepository<Dominus.Project.Project, Guid> _projectRepository;

    public ProjectFollowerAppService(
        IRepository<Dominus.ProjectFollower.ProjectFollower, Guid> repository,
        IRepository<Dominus.Project.Project, Guid> projectRepository
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Gets a single ProjectFollower by Id
    /// </summary>
    public virtual async Task<ProjectFollowerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProjectFollower.ProjectFollower, ProjectFollowerDto>(entity);
        if (entity.ProjectId != null)
        {
            var parent = await _projectRepository.FindAsync(entity.ProjectId.Value);
            dto.ProjectDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProjectFollowers
    /// </summary>
    public virtual async Task<PagedResultDto<ProjectFollowerDto>> GetListAsync(ProjectFollowerGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProjectFollower.ProjectFollower>, List<ProjectFollowerDto>>(entities);
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

        return new PagedResultDto<ProjectFollowerDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProjectFollower
    /// </summary>
    [Authorize(ProjectFollowerPermissions.Create)]
    public virtual async Task<ProjectFollowerDto> CreateAsync(CreateUpdateProjectFollowerDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProjectFollowerDto, Dominus.ProjectFollower.ProjectFollower>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectFollower.ProjectFollower, ProjectFollowerDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProjectFollower
    /// </summary>
    [Authorize(ProjectFollowerPermissions.Update)]
    public virtual async Task<ProjectFollowerDto> UpdateAsync(Guid id, CreateUpdateProjectFollowerDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProjectFollower.ProjectFollower), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectFollower.ProjectFollower, ProjectFollowerDto>(entity);
    }

    /// <summary>
    /// Deletes a ProjectFollower
    /// </summary>
    [Authorize(ProjectFollowerPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProjectFollowerLookupAsync()
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
    protected virtual IQueryable<Dominus.ProjectFollower.ProjectFollower> ApplyFilters(IQueryable<Dominus.ProjectFollower.ProjectFollower> queryable, ProjectFollowerGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProjectId != null, x => x.ProjectId == input.ProjectId)
            ;
    }
}
